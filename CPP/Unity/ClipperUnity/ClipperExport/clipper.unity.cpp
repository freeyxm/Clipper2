/*******************************************************************************
* Author    :  freeyxm                                                         *
* Date      :  2023.02.10                                                      *
* Website   :  https://github.com/freeyxm                                      *
* Copyright :                                                                  *
* Purpose   :  This module exports the Clipper2 Library (ie DLL/so)            *
* License   :                                                                  *
*******************************************************************************/
#include "clipper.unity.h"

namespace Clipper2Lib::Unity {

#pragma region Paths64
    static Paths64 ToPaths64(const CPaths64& pathsData);
    static CPaths64 ToCPaths64(const Paths64& paths);

    CPaths64 Intersect(const CPaths64& subjects, const CPaths64& clips, int fillrule)
    {
        Paths64 _subjects = ToPaths64(subjects);
        Paths64 _clips = ToPaths64(clips);
        Paths64 result = Intersect(_subjects, _clips, (FillRule)fillrule);
        return ToCPaths64(result);
    }

    CPaths64 Union(const CPaths64& subjects, int fillrule)
    {
        Paths64 _subjects = ToPaths64(subjects);
        Paths64 result = Union(_subjects, (FillRule)fillrule);
        return ToCPaths64(result);
    }

    CPaths64 Union2(const CPaths64& subjects, const CPaths64& clips, int fillrule)
    {
        Paths64 _subjects = ToPaths64(subjects);
        Paths64 _clips = ToPaths64(clips);
        Paths64 result = Union(_subjects, _clips, (FillRule)fillrule);
        return ToCPaths64(result);
    }

    CPaths64 Difference(const CPaths64& subjects, const CPaths64& clips, int fillrule)
    {
        Paths64 _subjects = ToPaths64(subjects);
        Paths64 _clips = ToPaths64(clips);
        Paths64 result = Difference(_subjects, _clips, (FillRule)fillrule);
        return ToCPaths64(result);
    }

    CPaths64 Xor(const CPaths64& subjects, const CPaths64& clips, int fillrule)
    {
        Paths64 _subjects = ToPaths64(subjects);
        Paths64 _clips = ToPaths64(clips);
        Paths64 result = Xor(_subjects, _clips, (FillRule)fillrule);
        return ToCPaths64(result);
    }

    Paths64 ToPaths64(const CPaths64& pathsData)
    {
        Paths64 paths;
        int64_t offset = 0;
        for (int64_t i = 0; i < pathsData.pathNum; ++i)
        {
            auto size = pathsData.sizePtr[i];
            auto data = pathsData.dataPtr + offset;
            Path64 path(data, data + size);
            paths.emplace_back(path);
            offset += size;
        }
        return paths;
    }

    CPaths64 ToCPaths64(const Paths64& paths)
    {
        CPaths64 data;
        data.pathNum = paths.size();
        data.sizePtr = new int64_t[data.pathNum];

        int64_t totalSize = 0;
        for (int64_t i = 0; i < data.pathNum; ++i)
        {
            const auto& list = paths[i];
            auto size = list.size();
            data.sizePtr[i] = size;
            totalSize += size;
        }

        data.dataPtr = new Point64[totalSize];
        auto dataPtr = data.dataPtr;
        for (int64_t i = 0; i < data.pathNum; ++i)
        {
            const auto& list = paths[i];
            auto size = list.size();
            ::memcpy(dataPtr, &list[0], sizeof(Point64) * size);
            dataPtr += size;
        }

        return data;
    }

    void ReleaseCPaths64(CPaths64& data) {
        delete[] data.dataPtr;
        delete[] data.sizePtr;
        data.dataPtr = nullptr;
        data.sizePtr = nullptr;
        data.pathNum = 0;
    }
#pragma endregion

#pragma region PathsD
    static PathsD ToPathsD(const CPathsD& pathsData);
    static CPathsD ToCPathsD(const PathsD& paths);

    CPathsD Intersect_D(const CPathsD& subjects, const CPathsD& clips, int fillrule, int precision)
    {
        PathsD _subjects = ToPathsD(subjects);
        PathsD _clips = ToPathsD(clips);
        PathsD result = Intersect(_subjects, _clips, (FillRule)fillrule, precision);
        return ToCPathsD(result);
    }

    CPathsD Union_D(const CPathsD& subjects, int fillrule, int precision)
    {
        PathsD _subjects = ToPathsD(subjects);
        PathsD result = Union(_subjects, (FillRule)fillrule, precision);
        return ToCPathsD(result);
    }

    CPathsD Union2_D(const CPathsD& subjects, const CPathsD& clips, int fillrule, int precision)
    {
        PathsD _subjects = ToPathsD(subjects);
        PathsD _clips = ToPathsD(clips);
        PathsD result = Union(_subjects, _clips, (FillRule)fillrule, precision);
        return ToCPathsD(result);
    }

    CPathsD Difference_D(const CPathsD& subjects, const CPathsD& clips, int fillrule, int precision)
    {
        PathsD _subjects = ToPathsD(subjects);
        PathsD _clips = ToPathsD(clips);
        PathsD result = Difference(_subjects, _clips, (FillRule)fillrule, precision);
        return ToCPathsD(result);
    }

    CPathsD Xor_D(const CPathsD& subjects, const CPathsD& clips, int fillrule, int precision)
    {
        PathsD _subjects = ToPathsD(subjects);
        PathsD _clips = ToPathsD(clips);
        PathsD result = Xor(_subjects, _clips, (FillRule)fillrule, precision);
        return ToCPathsD(result);
    }

    PathsD ToPathsD(const CPathsD& pathsData)
    {
        PathsD paths;
        int64_t offset = 0;
        for (int64_t i = 0; i < pathsData.pathNum; ++i)
        {
            auto size = pathsData.sizePtr[i];
            auto data = pathsData.dataPtr + offset;
            PathD path(data, data + size);
            paths.emplace_back(path);
            offset += size;
        }
        return paths;
    }

    CPathsD ToCPathsD(const PathsD& paths)
    {
        CPathsD data;
        data.pathNum = paths.size();
        data.sizePtr = new int64_t[data.pathNum];

        int64_t totalSize = 0;
        for (int64_t i = 0; i < data.pathNum; ++i)
        {
            const auto& list = paths[i];
            auto size = list.size();
            data.sizePtr[i] = size;
            totalSize += size;
        }

        data.dataPtr = new PointD[totalSize];
        auto dataPtr = data.dataPtr;
        for (int64_t i = 0; i < data.pathNum; ++i)
        {
            const auto& list = paths[i];
            auto size = list.size();
            ::memcpy(dataPtr, &list[0], sizeof(PointD) * size);
            dataPtr += size;
        }

        return data;
    }

    void ReleaseCPathsD(CPathsD& data) {
        delete[] data.dataPtr;
        delete[] data.sizePtr;
        data.dataPtr = nullptr;
        data.sizePtr = nullptr;
        data.pathNum = 0;
    }
#pragma endregion

}
