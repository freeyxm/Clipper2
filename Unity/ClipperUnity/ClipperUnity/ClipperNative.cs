/*******************************************************************************
* Author    :  freeyxm                                                         *
* Date      :  2023.02.10                                                      *
* Website   :  https://github.com/freeyxm                                      *
* Copyright :                                                                  *
* Purpose   :  DllImport for the Clipper2 Library                              *
* License   :                                                                  *
*******************************************************************************/
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Clipper2Lib.Native
{
    [StructLayout(LayoutKind.Sequential)]
    struct Point64
    {
        public long x;
        public long y;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct PointD
    {
        public double x;
        public double y;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct CPaths64
    {
        public long pathNum;
        public IntPtr sizePtr;
        public IntPtr dataPtr;
    };

    [StructLayout(LayoutKind.Sequential)]
    struct CPathsD
    {
        public long pathNum;
        public IntPtr sizePtr;
        public IntPtr dataPtr;
    };

    public static class ClipperNative
    {
        private const string DLL_NAME = "Clipper2";

        private class Paths64Data
        {
            public CPaths64 data;
            public long dataPtrSize = 0;
            public long sizePtrSize = 0;
        }

        private class PathsDData
        {
            public CPathsD data;
            public long dataPtrSize = 0;
            public long sizePtrSize = 0;
        }

        private static Paths64Data mSubjects64 = new Paths64Data();
        private static Paths64Data mClips64 = new Paths64Data();

        private static PathsDData mSubjectsD = new PathsDData();
        private static PathsDData mClipsD = new PathsDData();

        #region DllImport CPaths64
        [DllImport(DLL_NAME)]
        static extern CPaths64 Intersect(ref CPaths64 subjects, ref CPaths64 clips, int fillrule);

        [DllImport(DLL_NAME)]
        static extern CPaths64 Union(ref CPaths64 subjects, int fillrule);

        [DllImport(DLL_NAME)]
        static extern CPaths64 Union2(ref CPaths64 subjects, ref CPaths64 clips, int fillrule);

        [DllImport(DLL_NAME)]
        static extern CPaths64 Difference(ref CPaths64 subjects, ref CPaths64 clips, int fillrule);

        [DllImport(DLL_NAME)]
        static extern CPaths64 Xor(ref CPaths64 subjects, ref CPaths64 clips, int fillrule);

        [DllImport(DLL_NAME)]
        static extern void ReleaseCPaths64(ref CPaths64 data);
        #endregion

        #region DllImport CPathsD
        [DllImport(DLL_NAME)]
        static extern CPathsD Intersect_D(ref CPathsD subjects, ref CPathsD clips, int fillrule, int precision = 2);

        [DllImport(DLL_NAME)]
        static extern CPathsD Union_D(ref CPathsD subjects, int fillrule, int precision = 2);

        [DllImport(DLL_NAME)]
        static extern CPathsD Union2_D(ref CPathsD subjects, ref CPathsD clips, int fillrule, int precision = 2);

        [DllImport(DLL_NAME)]
        static extern CPathsD Difference_D(ref CPathsD subjects, ref CPathsD clips, int fillrule, int precision = 2);

        [DllImport(DLL_NAME)]
        static extern CPathsD Xor_D(ref CPathsD subjects, ref CPathsD clips, int fillrule, int precision = 2);

        [DllImport(DLL_NAME)]
        static extern void ReleaseCPathsD(ref CPathsD data);
        #endregion

        #region DllImport Triangulate
        [DllImport(DLL_NAME)]
        static extern CPathsD Triangulate_EC(ref CPathsD data);
        #endregion

        #region Paths64
        public static void Intersect(Paths64 subjects, Paths64 clips, FillRule fillrule, ref Paths64 result)
        {
            UpdatePathsData(ref mSubjects64, subjects);
            UpdatePathsData(ref mClips64, clips);
            CPaths64 data = Intersect(ref mSubjects64.data, ref mClips64.data, (int)fillrule);
            ToPaths64(data, ref result);
            ReleaseCPaths64(ref data);
        }

        public static void Union(Paths64 subjects, Paths64 clips, FillRule fillrule, ref Paths64 result)
        {
            UpdatePathsData(ref mSubjects64, subjects);
            UpdatePathsData(ref mClips64, clips);
            CPaths64 data = Union2(ref mSubjects64.data, ref mClips64.data, (int)fillrule);
            ToPaths64(data, ref result);
            ReleaseCPaths64(ref data);
        }

        public static void Union(Paths64 subjects, FillRule fillrule, ref Paths64 result)
        {
            UpdatePathsData(ref mSubjects64, subjects);
            CPaths64 data = Union(ref mSubjects64.data, (int)fillrule);
            ToPaths64(data, ref result);
            ReleaseCPaths64(ref data);
        }

        public static void Difference(Paths64 subjects, Paths64 clips, FillRule fillrule, ref Paths64 result)
        {
            UpdatePathsData(ref mSubjects64, subjects);
            UpdatePathsData(ref mClips64, clips);
            CPaths64 data = Difference(ref mSubjects64.data, ref mClips64.data, (int)fillrule);
            ToPaths64(data, ref result);
            ReleaseCPaths64(ref data);
        }

        public static void Xor(Paths64 subjects, Paths64 clips, FillRule fillrule, ref Paths64 result)
        {
            UpdatePathsData(ref mSubjects64, subjects);
            UpdatePathsData(ref mClips64, clips);
            CPaths64 data = Xor(ref mSubjects64.data, ref mClips64.data, (int)fillrule);
            ToPaths64(data, ref result);
            ReleaseCPaths64(ref data);
        }
        #endregion

        #region PathsD
        public static void Intersect(PathsD subjects, PathsD clips, FillRule fillrule, ref PathsD result, int precision = 2)
        {
            UpdatePathsData(ref mSubjectsD, subjects);
            UpdatePathsData(ref mClipsD, clips);
            CPathsD data = Intersect_D(ref mSubjectsD.data, ref mClipsD.data, (int)fillrule, precision);
            ToPathsD(data, ref result);
            ReleaseCPathsD(ref data);
        }

        public static void Union(PathsD subjects, PathsD clips, FillRule fillrule, ref PathsD result, int precision = 2)
        {
            UpdatePathsData(ref mSubjectsD, subjects);
            UpdatePathsData(ref mClipsD, clips);
            CPathsD data = Union2_D(ref mSubjectsD.data, ref mClipsD.data, (int)fillrule, precision);
            ToPathsD(data, ref result);
            ReleaseCPathsD(ref data);
        }

        public static void Union(PathsD subjects, FillRule fillrule, ref PathsD result, int precision = 2)
        {
            UpdatePathsData(ref mSubjectsD, subjects);
            CPathsD data = Union_D(ref mSubjectsD.data, (int)fillrule, precision);
            ToPathsD(data, ref result);
            ReleaseCPathsD(ref data);
        }

        public static void Difference(PathsD subjects, PathsD clips, FillRule fillrule, ref PathsD result, int precision = 2)
        {
            UpdatePathsData(ref mSubjectsD, subjects);
            UpdatePathsData(ref mClipsD, clips);
            CPathsD data = Difference_D(ref mSubjectsD.data, ref mClipsD.data, (int)fillrule, precision);
            ToPathsD(data, ref result);
            ReleaseCPathsD(ref data);
        }

        public static void Xor(PathsD subjects, PathsD clips, FillRule fillrule, ref PathsD result, int precision = 2)
        {
            UpdatePathsData(ref mSubjectsD, subjects);
            UpdatePathsData(ref mClipsD, clips);
            CPathsD data = Xor_D(ref mSubjectsD.data, ref mClipsD.data, (int)fillrule, precision);
            ToPathsD(data, ref result);
            ReleaseCPathsD(ref data);
        }
        #endregion

        #region Triangulate
        public static void Triangulate_EC(PathsD paths, ref PathsD result)
        {
            UpdatePathsData(ref mSubjectsD, paths);
            CPathsD output = Triangulate_EC(ref mSubjectsD.data);
            ToPathsD(output, ref result);
            ReleaseCPathsD(ref output);
        }
        #endregion

        public static void Release()
        {
            ReleasePathsData(mSubjects64);
            ReleasePathsData(mClips64);
            ReleasePathsData(mSubjectsD);
            ReleasePathsData(mClipsD);
            ClipperPathPool.Release();
        }

        #region inner
        private static void ReleasePathsData(Paths64Data pathsData)
        {
            if (pathsData.data.dataPtr != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(pathsData.data.dataPtr);
                pathsData.data.dataPtr = IntPtr.Zero;
            }
            if (pathsData.data.sizePtr != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(pathsData.data.sizePtr);
                pathsData.data.sizePtr = IntPtr.Zero;
            }
            pathsData.dataPtrSize = 0;
            pathsData.sizePtrSize = 0;
        }

        private static void ReleasePathsData(PathsDData pathsData)
        {
            if (pathsData.data.dataPtr != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(pathsData.data.dataPtr);
                pathsData.data.dataPtr = IntPtr.Zero;
            }
            if (pathsData.data.sizePtr != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(pathsData.data.sizePtr);
                pathsData.data.sizePtr = IntPtr.Zero;
            }
            pathsData.dataPtrSize = 0;
            pathsData.sizePtrSize = 0;
        }

        private static void UpdatePathsData(ref Paths64Data pathsData, Paths64 paths)
        {
            int pathNum = paths.Count;
            int dataSize = 0;
            for (int i = 0; i < pathNum; ++i)
            {
                dataSize += paths[i].Count;
            }

            if (pathsData.dataPtrSize < dataSize)
            {
                int cb = Marshal.SizeOf(typeof(Point64)) * dataSize;
                if (pathsData.data.dataPtr != IntPtr.Zero)
                    pathsData.data.dataPtr = Marshal.ReAllocHGlobal(pathsData.data.dataPtr, new IntPtr(cb));
                else
                    pathsData.data.dataPtr = Marshal.AllocHGlobal(cb);
                pathsData.dataPtrSize = dataSize;
            }

            if (pathsData.sizePtrSize < pathNum)
            {
                int cb = Marshal.SizeOf(typeof(long)) * pathNum;
                if (pathsData.data.sizePtr != IntPtr.Zero)
                    pathsData.data.sizePtr = Marshal.ReAllocHGlobal(pathsData.data.sizePtr, new IntPtr(cb));
                else
                    pathsData.data.sizePtr = Marshal.AllocHGlobal(cb);
                pathsData.sizePtrSize = pathNum;
            }

            pathsData.data.pathNum = pathNum;

            unsafe
            {
                var dataPtr = (Point64*)pathsData.data.dataPtr;
                var sizePtr = (long*)pathsData.data.sizePtr;
                for (int i = 0; i < pathNum; ++i)
                {
                    var path = paths[i];
                    *sizePtr++ = path.Count;
                    for (int j = 0; j < path.Count; ++j)
                    {
                        var point = path[j];
                        dataPtr->x = point.X;
                        dataPtr->y = point.Y;
                        dataPtr++;
                    }
                }
            }
        }

        private static void UpdatePathsData(ref PathsDData pathsData, PathsD paths)
        {
            int pathNum = paths.Count;
            int dataSize = 0;
            for (int i = 0; i < pathNum; ++i)
            {
                dataSize += paths[i].Count;
            }

            if (pathsData.dataPtrSize < dataSize)
            {
                int cb = Marshal.SizeOf(typeof(PointD)) * dataSize;
                if (pathsData.data.dataPtr != IntPtr.Zero)
                    pathsData.data.dataPtr = Marshal.ReAllocHGlobal(pathsData.data.dataPtr, new IntPtr(cb));
                else
                    pathsData.data.dataPtr = Marshal.AllocHGlobal(cb);
                pathsData.dataPtrSize = dataSize;
            }

            if (pathsData.sizePtrSize < pathNum)
            {
                int cb = Marshal.SizeOf(typeof(long)) * pathNum;
                if (pathsData.data.sizePtr != IntPtr.Zero)
                    pathsData.data.sizePtr = Marshal.ReAllocHGlobal(pathsData.data.sizePtr, new IntPtr(cb));
                else
                    pathsData.data.sizePtr = Marshal.AllocHGlobal(cb);
                pathsData.sizePtrSize = pathNum;
            }

            pathsData.data.pathNum = pathNum;

            unsafe
            {
                var dataPtr = (PointD*)pathsData.data.dataPtr;
                var sizePtr = (long*)pathsData.data.sizePtr;
                for (int i = 0; i < pathNum; ++i)
                {
                    var path = paths[i];
                    *sizePtr++ = path.Count;
                    for (int j = 0; j < path.Count; ++j)
                    {
                        var point = path[j];
                        dataPtr->x = point.x;
                        dataPtr->y = point.y;
                        dataPtr++;
                    }
                }
            }
        }

        private static void ToPaths64(CPaths64 pathsData, ref Paths64 paths)
        {
            unsafe
            {
                var dataPtr = (Point64*)pathsData.dataPtr;
                var sizePtr = (long*)pathsData.sizePtr;
                int capacity = (int)(paths.Count + pathsData.pathNum);
                if (paths.Capacity < capacity)
                {
                    paths.Capacity = capacity;
                }
                for (int i = 0; i < pathsData.pathNum; ++i)
                {
                    long size = *sizePtr++;
                    var path = ClipperPathPool.mPath64Pool.Get((int)size);
                    for (int j = 0; j < size; ++j)
                    {
                        path.Add(new Clipper2Lib.Point64(dataPtr->x, dataPtr->y));
                        dataPtr++;
                    }
                    paths.Add(path);
                }
            }
        }

        private static void ToPathsD(CPathsD pathsData, ref PathsD paths)
        {
            unsafe
            {
                var dataPtr = (PointD*)pathsData.dataPtr;
                var sizePtr = (long*)pathsData.sizePtr;
                int capacity = (int)(paths.Count + pathsData.pathNum);
                if (paths.Capacity < capacity)
                {
                    paths.Capacity = capacity;
                }
                for (int i = 0; i < pathsData.pathNum; ++i)
                {
                    long size = *sizePtr++;
                    var path = ClipperPathPool.mPathDPool.Get((int)size);
                    for (int j = 0; j < size; ++j)
                    {
                        path.Add(new Clipper2Lib.PointD(dataPtr->x, dataPtr->y));
                        dataPtr++;
                    }
                    paths.Add(path);
                }
            }
        }
        #endregion
    }

    public static class ClipperPathPool
    {
        public static readonly IPathPool<Paths64, Path64> mPaths64Pool;
        public static readonly IPathPool<PathsD, PathD> mPathsDPool;
        public static readonly IPathPool<Path64, Clipper2Lib.Point64> mPath64Pool;
        public static readonly IPathPool<PathD, Clipper2Lib.PointD> mPathDPool;

        static ClipperPathPool()
        {
            mPaths64Pool = new PathPool<Paths64, Path64>();
            mPathsDPool = new PathPool<PathsD, PathD>();
            mPath64Pool = new PathPool<Path64, Clipper2Lib.Point64>();
            mPathDPool = new PathPool<PathD, Clipper2Lib.PointD>();
        }

        public static void RecyclePaths(ref Paths64 paths)
        {
            if (paths != null)
            {
                for (int i = 0; i < paths.Count; ++i)
                {
                    mPath64Pool.Recycle(paths[i]);
                }
                paths.Clear();
                mPaths64Pool.Recycle(paths);
                paths = null;
            }
        }

        public static void RecyclePaths(ref PathsD paths)
        {
            if (paths != null)
            {
                for (int i = 0; i < paths.Count; ++i)
                {
                    mPathDPool.Recycle(paths[i]);
                }
                paths.Clear();
                mPathsDPool.Recycle(paths);
                paths = null;
            }
        }

        public static void Release()
        {
            mPaths64Pool.Release();
            mPathsDPool.Release();
            mPath64Pool.Release();
            mPathDPool.Release();
        }
    }

    #region PathPool
    public interface IPathPool<T, T2> where T : List<T2>, new()
    {
#if DEBUG
        int AllocCount { get; }
#endif
        int Count { get; }
        T Get(int capacity = 0);
        void Recycle(T obj);
        void Clear();
        void Release();
    }

    public class PathPool<T, T2> : IPathPool<T, T2> where T : List<T2>, new()
    {
        private Stack<T> mStack;
        private int mCapacity;
#if DEBUG
        private int mAllocCount;
        public int AllocCount => mAllocCount;
#endif
        public int Count => mStack.Count;
        public int Capacity => mCapacity;

        public PathPool(int capacity = 0)
        {
            mCapacity = capacity;
            mStack = new Stack<T>(mCapacity);
        }

        public T Get(int capacity = 0)
        {
            T result;
            if (mStack.Count > 0)
            {
                result = mStack.Pop();
            }
            else
            {
                result = new T();
#if DEBUG
                ++mAllocCount;
#endif
            }

            if (result.Capacity < capacity)
                result.Capacity = capacity;
            return result;
        }

        public void Recycle(T obj)
        {
#if DEBUG
            if (mStack.Contains(obj))
            {
                throw new Exception($"{this} Recycle repeat!");
            }
            else
#endif
            {
                obj.Clear();
                mStack.Push(obj);
            }
        }

        public void Clear()
        {
            mStack.Clear();
#if DEBUG
            mAllocCount = 0;
#endif
        }

        public void Release()
        {
            mStack = new Stack<T>(mCapacity);
#if DEBUG
            mAllocCount = 0;
#endif
        }
    }

    public class BetterPathPool<T, T2> : IPathPool<T, T2> where T : List<T2>, new()
    {
        private MultiValueDictionary<int, T> mDict;
        private List<int> mKeyList;
#if DEBUG
        private int mAllocCount;
        public int AllocCount => mAllocCount;
#endif
        public int Count => mDict.Count;

        public BetterPathPool()
        {
            mDict = new MultiValueDictionary<int, T>();
            mKeyList = new List<int>();
        }

        public T Get(int capacity = 0)
        {
            T result;
            if (mDict.Count > 0)
            {
                T value;
                if (mDict.TryPopValue(capacity, out value))
                    return value;

                mKeyList.Clear();
                mKeyList.AddRange(mDict.Keys);
                mKeyList.Sort();

                int bestKey;
                if (capacity == 0)
                {
                    bestKey = mKeyList[0];
                }
                else
                {
                    int left = 0;
                    int right = mKeyList.Count - 1;
                    int mid = (left + right) / 2;
                    while (left < right)
                    {
                        int key = mKeyList[mid];
                        if (key < capacity)
                            left = mid + 1;
                        else
                            right = mid;
                        mid = (left + right) / 2;
                    }
                    bestKey = mKeyList[mid];
                }

                if (mDict.TryPopValue(bestKey, out value))
                {
                    result = value;
                }
                else
                {
                    result = new T();
#if DEBUG
                    ++mAllocCount;
#endif
                }
            }
            else
            {
                result = new T();
#if DEBUG
                ++mAllocCount;
#endif
            }

            if (result.Capacity < capacity)
                result.Capacity = capacity;
            return result;
        }

        public void Recycle(T obj)
        {
#if DEBUG
            if (mDict.ContainsValue(obj))
            {
                throw new Exception($"{this} Recycle repeat!");
            }
            else
#endif
            {
                obj.Clear();
                mDict.Add(obj.Capacity, obj);
            }
        }

        public void Clear()
        {
            mDict.Clear();
            mKeyList.Clear();
#if DEBUG
            mAllocCount = 0;
#endif
        }

        public void Release()
        {
            mDict = new MultiValueDictionary<int, T>();
            mKeyList = new List<int>();
#if DEBUG
            mAllocCount = 0;
#endif
        }
    }
    #endregion
}
