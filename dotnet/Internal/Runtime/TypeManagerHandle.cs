// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Runtime;
using System.Runtime.InteropServices;

namespace Internal.Runtime
{
    /// <summary>
    /// TypeManagerHandle represents an AOT module in MRT based runtimes.
    /// These handles are a pointer to a TypeManager
    /// </summary>
    public unsafe partial struct TypeManagerHandle
    {
        private TypeManager* _handleValue;

        public TypeManagerHandle(IntPtr handleValue)
        {
            _handleValue = (TypeManager*)handleValue;
        }

        public IntPtr GetIntPtrUNSAFE()
        {
            return (IntPtr)_handleValue;
        }

        public bool IsNull
        {
            get
            {
                return _handleValue == null;
            }
        }

        internal unsafe TypeManager* AsTypeManager() => _handleValue;

        public unsafe IntPtr OsModuleBase
        {
            get
            {
                return _handleValue->OsHandle;
            }
        }
    }

    // This is a partial definition of the TypeManager struct which is defined in TypeManager.h
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct TypeManager
    {
        public IntPtr                      OsHandle;
        public ReadyToRunHeader *          ReadyToRunHeader;
        private byte*                    m_pStaticsGCDataSection;
        private byte*                    m_pThreadStaticsDataSection;
        private void**                      m_pClasslibFunctions;
        private uint                    m_nClasslibFunctions;

        public IntPtr GetModuleSection(ReadyToRunSectionType sectionId, int* length) 
        {
            ModuleInfoRow* pModuleInfoRows = (ModuleInfoRow*)(ReadyToRunHeader + 1);

            // TODO: Binary search
            for (int i = 0; i < ReadyToRunHeader->NumberOfSections; i++)
            {
                ModuleInfoRow* pCurrent = pModuleInfoRows + i;
                if ((int)sectionId == pCurrent->SectionId)
                {
                    *length = pCurrent->Length;
                    return pCurrent->Start;
                }
            }

            *length = 0;
            return IntPtr.Zero;
        }

        public void* GetClasslibFunction(ClassLibFunctionId functionId)
        {
            uint id = (uint)functionId;

            if (id >= m_nClasslibFunctions)
                return (void*)0;

            return m_pClasslibFunctions[id];
        }
    }
}
