/*******************************************************************************
* Author    :  freeyxm                                                         *
* Date      :  2023.02.10                                                      *
* Website   :  https://github.com/freeyxm                                      *
* Copyright :                                                                  *
* Purpose   :  This module exports the Clipper2 Library (ie DLL/so)            *
* License   :                                                                  *
*******************************************************************************/
#pragma once
#include "clipper2/clipper.h"
#define DllExport extern "C" __declspec(dllexport)

namespace Clipper2Lib::Unity {

    struct CPaths64
    {
        int64_t pathNum;
        int64_t* sizePtr;
        Point64* dataPtr;
    };

    struct CPathsD
    {
        int64_t pathNum;
        int64_t* sizePtr;
        PointD* dataPtr;
    };

    DllExport CPaths64 Intersect(const CPaths64& subjects, const CPaths64& clips, int fillrule);
    DllExport CPaths64 Union(const CPaths64& subjects, int fillrule);
    DllExport CPaths64 Union2(const CPaths64& subjects, const CPaths64& clips, int fillrule);
    DllExport CPaths64 Difference(const CPaths64& subjects, const CPaths64& clips, int fillrule);
    DllExport CPaths64 Xor(const CPaths64& subjects, const CPaths64& clips, int fillrule);
    DllExport void ReleaseCPaths64(CPaths64& data);

    DllExport CPathsD Intersect_D(const CPathsD& subjects, const CPathsD& clips, int fillrule, int precision = 2);
    DllExport CPathsD Union_D(const CPathsD& subjects, int fillrule, int precision = 2);
    DllExport CPathsD Union2_D(const CPathsD& subjects, const CPathsD& clips, int fillrule, int precision = 2);
    DllExport CPathsD Difference_D(const CPathsD& subjects, const CPathsD& clips, int fillrule, int precision = 2);
    DllExport CPathsD Xor_D(const CPathsD& subjects, const CPathsD& clips, int fillrule, int precision = 2);
    DllExport void ReleaseCPathsD(CPathsD& data);

    DllExport CPathsD Triangulate_EC(const CPathsD& data);
}
