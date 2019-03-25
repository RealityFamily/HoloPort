#include "il2cpp-config.h"

#ifndef _MSC_VER
# include <alloca.h>
#else
# include <malloc.h>
#endif


#include <cstring>
#include <string.h>
#include <stdio.h>
#include <cmath>
#include <limits>
#include <assert.h>
#include <stdint.h>

#include "il2cpp-class-internals.h"
#include "codegen/il2cpp-codegen.h"
#include "il2cpp-object-internals.h"


// System.Char[]
struct CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2;
// System.Collections.IDictionary
struct IDictionary_t1BD5C1546718A374EA8122FBD6C6EE45331E8CE7;
// System.Diagnostics.StackTrace[]
struct StackTraceU5BU5D_t855F09649EA34DEE7C1B6F088E0538E3CCC3F196;
// System.IntPtr[]
struct IntPtrU5BU5D_t4DC01DCB9A6DF6C9792A6513595D7A11E637DCDD;
// System.Runtime.Serialization.SafeSerializationManager
struct SafeSerializationManager_t4A754D86B0F784B18CBC36C073BA564BED109770;
// System.String
struct String_t;
// System.Void
struct Void_t22962CB4C05B1D89B55A6E1139F0E87A90987017;
// UnityEngine.Transform
struct Transform_tBB9E78A2766C3C83599A8F66EDE7D1FCAFC66EDA;

struct Exception_t_marshaled_com;
struct Exception_t_marshaled_pinvoke;



#ifndef RUNTIMEOBJECT_H
#define RUNTIMEOBJECT_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Object

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // RUNTIMEOBJECT_H
#ifndef BOUNDSEXTENSION_T0CDA9EFB63602DAF0A2FF8702DD9143A062CB613_H
#define BOUNDSEXTENSION_T0CDA9EFB63602DAF0A2FF8702DD9143A062CB613_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// HoloGroup.Geometry.Extensions.BoundsExtension
struct  BoundsExtension_t0CDA9EFB63602DAF0A2FF8702DD9143A062CB613  : public RuntimeObject
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // BOUNDSEXTENSION_T0CDA9EFB63602DAF0A2FF8702DD9143A062CB613_H
#ifndef MATRIXEXTENSIONS_T367F29DBDD3C67BA169CA1B8677E181756ABA5D1_H
#define MATRIXEXTENSIONS_T367F29DBDD3C67BA169CA1B8677E181756ABA5D1_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// HoloGroup.Geometry.Extensions.MatrixExtensions
struct  MatrixExtensions_t367F29DBDD3C67BA169CA1B8677E181756ABA5D1  : public RuntimeObject
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // MATRIXEXTENSIONS_T367F29DBDD3C67BA169CA1B8677E181756ABA5D1_H
#ifndef EXCEPTION_T_H
#define EXCEPTION_T_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Exception
struct  Exception_t  : public RuntimeObject
{
public:
	// System.String System.Exception::_className
	String_t* ____className_1;
	// System.String System.Exception::_message
	String_t* ____message_2;
	// System.Collections.IDictionary System.Exception::_data
	RuntimeObject* ____data_3;
	// System.Exception System.Exception::_innerException
	Exception_t * ____innerException_4;
	// System.String System.Exception::_helpURL
	String_t* ____helpURL_5;
	// System.Object System.Exception::_stackTrace
	RuntimeObject * ____stackTrace_6;
	// System.String System.Exception::_stackTraceString
	String_t* ____stackTraceString_7;
	// System.String System.Exception::_remoteStackTraceString
	String_t* ____remoteStackTraceString_8;
	// System.Int32 System.Exception::_remoteStackIndex
	int32_t ____remoteStackIndex_9;
	// System.Object System.Exception::_dynamicMethods
	RuntimeObject * ____dynamicMethods_10;
	// System.Int32 System.Exception::_HResult
	int32_t ____HResult_11;
	// System.String System.Exception::_source
	String_t* ____source_12;
	// System.Runtime.Serialization.SafeSerializationManager System.Exception::_safeSerializationManager
	SafeSerializationManager_t4A754D86B0F784B18CBC36C073BA564BED109770 * ____safeSerializationManager_13;
	// System.Diagnostics.StackTrace[] System.Exception::captured_traces
	StackTraceU5BU5D_t855F09649EA34DEE7C1B6F088E0538E3CCC3F196* ___captured_traces_14;
	// System.IntPtr[] System.Exception::native_trace_ips
	IntPtrU5BU5D_t4DC01DCB9A6DF6C9792A6513595D7A11E637DCDD* ___native_trace_ips_15;

public:
	inline static int32_t get_offset_of__className_1() { return static_cast<int32_t>(offsetof(Exception_t, ____className_1)); }
	inline String_t* get__className_1() const { return ____className_1; }
	inline String_t** get_address_of__className_1() { return &____className_1; }
	inline void set__className_1(String_t* value)
	{
		____className_1 = value;
		Il2CppCodeGenWriteBarrier((&____className_1), value);
	}

	inline static int32_t get_offset_of__message_2() { return static_cast<int32_t>(offsetof(Exception_t, ____message_2)); }
	inline String_t* get__message_2() const { return ____message_2; }
	inline String_t** get_address_of__message_2() { return &____message_2; }
	inline void set__message_2(String_t* value)
	{
		____message_2 = value;
		Il2CppCodeGenWriteBarrier((&____message_2), value);
	}

	inline static int32_t get_offset_of__data_3() { return static_cast<int32_t>(offsetof(Exception_t, ____data_3)); }
	inline RuntimeObject* get__data_3() const { return ____data_3; }
	inline RuntimeObject** get_address_of__data_3() { return &____data_3; }
	inline void set__data_3(RuntimeObject* value)
	{
		____data_3 = value;
		Il2CppCodeGenWriteBarrier((&____data_3), value);
	}

	inline static int32_t get_offset_of__innerException_4() { return static_cast<int32_t>(offsetof(Exception_t, ____innerException_4)); }
	inline Exception_t * get__innerException_4() const { return ____innerException_4; }
	inline Exception_t ** get_address_of__innerException_4() { return &____innerException_4; }
	inline void set__innerException_4(Exception_t * value)
	{
		____innerException_4 = value;
		Il2CppCodeGenWriteBarrier((&____innerException_4), value);
	}

	inline static int32_t get_offset_of__helpURL_5() { return static_cast<int32_t>(offsetof(Exception_t, ____helpURL_5)); }
	inline String_t* get__helpURL_5() const { return ____helpURL_5; }
	inline String_t** get_address_of__helpURL_5() { return &____helpURL_5; }
	inline void set__helpURL_5(String_t* value)
	{
		____helpURL_5 = value;
		Il2CppCodeGenWriteBarrier((&____helpURL_5), value);
	}

	inline static int32_t get_offset_of__stackTrace_6() { return static_cast<int32_t>(offsetof(Exception_t, ____stackTrace_6)); }
	inline RuntimeObject * get__stackTrace_6() const { return ____stackTrace_6; }
	inline RuntimeObject ** get_address_of__stackTrace_6() { return &____stackTrace_6; }
	inline void set__stackTrace_6(RuntimeObject * value)
	{
		____stackTrace_6 = value;
		Il2CppCodeGenWriteBarrier((&____stackTrace_6), value);
	}

	inline static int32_t get_offset_of__stackTraceString_7() { return static_cast<int32_t>(offsetof(Exception_t, ____stackTraceString_7)); }
	inline String_t* get__stackTraceString_7() const { return ____stackTraceString_7; }
	inline String_t** get_address_of__stackTraceString_7() { return &____stackTraceString_7; }
	inline void set__stackTraceString_7(String_t* value)
	{
		____stackTraceString_7 = value;
		Il2CppCodeGenWriteBarrier((&____stackTraceString_7), value);
	}

	inline static int32_t get_offset_of__remoteStackTraceString_8() { return static_cast<int32_t>(offsetof(Exception_t, ____remoteStackTraceString_8)); }
	inline String_t* get__remoteStackTraceString_8() const { return ____remoteStackTraceString_8; }
	inline String_t** get_address_of__remoteStackTraceString_8() { return &____remoteStackTraceString_8; }
	inline void set__remoteStackTraceString_8(String_t* value)
	{
		____remoteStackTraceString_8 = value;
		Il2CppCodeGenWriteBarrier((&____remoteStackTraceString_8), value);
	}

	inline static int32_t get_offset_of__remoteStackIndex_9() { return static_cast<int32_t>(offsetof(Exception_t, ____remoteStackIndex_9)); }
	inline int32_t get__remoteStackIndex_9() const { return ____remoteStackIndex_9; }
	inline int32_t* get_address_of__remoteStackIndex_9() { return &____remoteStackIndex_9; }
	inline void set__remoteStackIndex_9(int32_t value)
	{
		____remoteStackIndex_9 = value;
	}

	inline static int32_t get_offset_of__dynamicMethods_10() { return static_cast<int32_t>(offsetof(Exception_t, ____dynamicMethods_10)); }
	inline RuntimeObject * get__dynamicMethods_10() const { return ____dynamicMethods_10; }
	inline RuntimeObject ** get_address_of__dynamicMethods_10() { return &____dynamicMethods_10; }
	inline void set__dynamicMethods_10(RuntimeObject * value)
	{
		____dynamicMethods_10 = value;
		Il2CppCodeGenWriteBarrier((&____dynamicMethods_10), value);
	}

	inline static int32_t get_offset_of__HResult_11() { return static_cast<int32_t>(offsetof(Exception_t, ____HResult_11)); }
	inline int32_t get__HResult_11() const { return ____HResult_11; }
	inline int32_t* get_address_of__HResult_11() { return &____HResult_11; }
	inline void set__HResult_11(int32_t value)
	{
		____HResult_11 = value;
	}

	inline static int32_t get_offset_of__source_12() { return static_cast<int32_t>(offsetof(Exception_t, ____source_12)); }
	inline String_t* get__source_12() const { return ____source_12; }
	inline String_t** get_address_of__source_12() { return &____source_12; }
	inline void set__source_12(String_t* value)
	{
		____source_12 = value;
		Il2CppCodeGenWriteBarrier((&____source_12), value);
	}

	inline static int32_t get_offset_of__safeSerializationManager_13() { return static_cast<int32_t>(offsetof(Exception_t, ____safeSerializationManager_13)); }
	inline SafeSerializationManager_t4A754D86B0F784B18CBC36C073BA564BED109770 * get__safeSerializationManager_13() const { return ____safeSerializationManager_13; }
	inline SafeSerializationManager_t4A754D86B0F784B18CBC36C073BA564BED109770 ** get_address_of__safeSerializationManager_13() { return &____safeSerializationManager_13; }
	inline void set__safeSerializationManager_13(SafeSerializationManager_t4A754D86B0F784B18CBC36C073BA564BED109770 * value)
	{
		____safeSerializationManager_13 = value;
		Il2CppCodeGenWriteBarrier((&____safeSerializationManager_13), value);
	}

	inline static int32_t get_offset_of_captured_traces_14() { return static_cast<int32_t>(offsetof(Exception_t, ___captured_traces_14)); }
	inline StackTraceU5BU5D_t855F09649EA34DEE7C1B6F088E0538E3CCC3F196* get_captured_traces_14() const { return ___captured_traces_14; }
	inline StackTraceU5BU5D_t855F09649EA34DEE7C1B6F088E0538E3CCC3F196** get_address_of_captured_traces_14() { return &___captured_traces_14; }
	inline void set_captured_traces_14(StackTraceU5BU5D_t855F09649EA34DEE7C1B6F088E0538E3CCC3F196* value)
	{
		___captured_traces_14 = value;
		Il2CppCodeGenWriteBarrier((&___captured_traces_14), value);
	}

	inline static int32_t get_offset_of_native_trace_ips_15() { return static_cast<int32_t>(offsetof(Exception_t, ___native_trace_ips_15)); }
	inline IntPtrU5BU5D_t4DC01DCB9A6DF6C9792A6513595D7A11E637DCDD* get_native_trace_ips_15() const { return ___native_trace_ips_15; }
	inline IntPtrU5BU5D_t4DC01DCB9A6DF6C9792A6513595D7A11E637DCDD** get_address_of_native_trace_ips_15() { return &___native_trace_ips_15; }
	inline void set_native_trace_ips_15(IntPtrU5BU5D_t4DC01DCB9A6DF6C9792A6513595D7A11E637DCDD* value)
	{
		___native_trace_ips_15 = value;
		Il2CppCodeGenWriteBarrier((&___native_trace_ips_15), value);
	}
};

struct Exception_t_StaticFields
{
public:
	// System.Object System.Exception::s_EDILock
	RuntimeObject * ___s_EDILock_0;

public:
	inline static int32_t get_offset_of_s_EDILock_0() { return static_cast<int32_t>(offsetof(Exception_t_StaticFields, ___s_EDILock_0)); }
	inline RuntimeObject * get_s_EDILock_0() const { return ___s_EDILock_0; }
	inline RuntimeObject ** get_address_of_s_EDILock_0() { return &___s_EDILock_0; }
	inline void set_s_EDILock_0(RuntimeObject * value)
	{
		___s_EDILock_0 = value;
		Il2CppCodeGenWriteBarrier((&___s_EDILock_0), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
// Native definition for P/Invoke marshalling of System.Exception
struct Exception_t_marshaled_pinvoke
{
	char* ____className_1;
	char* ____message_2;
	RuntimeObject* ____data_3;
	Exception_t_marshaled_pinvoke* ____innerException_4;
	char* ____helpURL_5;
	Il2CppIUnknown* ____stackTrace_6;
	char* ____stackTraceString_7;
	char* ____remoteStackTraceString_8;
	int32_t ____remoteStackIndex_9;
	Il2CppIUnknown* ____dynamicMethods_10;
	int32_t ____HResult_11;
	char* ____source_12;
	SafeSerializationManager_t4A754D86B0F784B18CBC36C073BA564BED109770 * ____safeSerializationManager_13;
	StackTraceU5BU5D_t855F09649EA34DEE7C1B6F088E0538E3CCC3F196* ___captured_traces_14;
	intptr_t* ___native_trace_ips_15;
};
// Native definition for COM marshalling of System.Exception
struct Exception_t_marshaled_com
{
	Il2CppChar* ____className_1;
	Il2CppChar* ____message_2;
	RuntimeObject* ____data_3;
	Exception_t_marshaled_com* ____innerException_4;
	Il2CppChar* ____helpURL_5;
	Il2CppIUnknown* ____stackTrace_6;
	Il2CppChar* ____stackTraceString_7;
	Il2CppChar* ____remoteStackTraceString_8;
	int32_t ____remoteStackIndex_9;
	Il2CppIUnknown* ____dynamicMethods_10;
	int32_t ____HResult_11;
	Il2CppChar* ____source_12;
	SafeSerializationManager_t4A754D86B0F784B18CBC36C073BA564BED109770 * ____safeSerializationManager_13;
	StackTraceU5BU5D_t855F09649EA34DEE7C1B6F088E0538E3CCC3F196* ___captured_traces_14;
	intptr_t* ___native_trace_ips_15;
};
#endif // EXCEPTION_T_H
#ifndef VALUETYPE_T4D0C27076F7C36E76190FB3328E232BCB1CD1FFF_H
#define VALUETYPE_T4D0C27076F7C36E76190FB3328E232BCB1CD1FFF_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.ValueType
struct  ValueType_t4D0C27076F7C36E76190FB3328E232BCB1CD1FFF  : public RuntimeObject
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
// Native definition for P/Invoke marshalling of System.ValueType
struct ValueType_t4D0C27076F7C36E76190FB3328E232BCB1CD1FFF_marshaled_pinvoke
{
};
// Native definition for COM marshalling of System.ValueType
struct ValueType_t4D0C27076F7C36E76190FB3328E232BCB1CD1FFF_marshaled_com
{
};
#endif // VALUETYPE_T4D0C27076F7C36E76190FB3328E232BCB1CD1FFF_H
#ifndef __STATICARRAYINITTYPESIZEU3D100_T8DB8E840BBC5746CC367785F67BA5F3C1589AA9D_H
#define __STATICARRAYINITTYPESIZEU3D100_T8DB8E840BBC5746CC367785F67BA5F3C1589AA9D_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// <PrivateImplementationDetails>___StaticArrayInitTypeSizeU3D100
struct  __StaticArrayInitTypeSizeU3D100_t8DB8E840BBC5746CC367785F67BA5F3C1589AA9D 
{
public:
	union
	{
		struct
		{
			union
			{
			};
		};
		uint8_t __StaticArrayInitTypeSizeU3D100_t8DB8E840BBC5746CC367785F67BA5F3C1589AA9D__padding[100];
	};

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // __STATICARRAYINITTYPESIZEU3D100_T8DB8E840BBC5746CC367785F67BA5F3C1589AA9D_H
#ifndef __STATICARRAYINITTYPESIZEU3D16_T6EB025C40E80FDD74132718092D59E92446BD13D_H
#define __STATICARRAYINITTYPESIZEU3D16_T6EB025C40E80FDD74132718092D59E92446BD13D_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// <PrivateImplementationDetails>___StaticArrayInitTypeSizeU3D16
struct  __StaticArrayInitTypeSizeU3D16_t6EB025C40E80FDD74132718092D59E92446BD13D 
{
public:
	union
	{
		struct
		{
			union
			{
			};
		};
		uint8_t __StaticArrayInitTypeSizeU3D16_t6EB025C40E80FDD74132718092D59E92446BD13D__padding[16];
	};

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // __STATICARRAYINITTYPESIZEU3D16_T6EB025C40E80FDD74132718092D59E92446BD13D_H
#ifndef __STATICARRAYINITTYPESIZEU3D24_T0186834C621050282F5EF637E836A6E1DC934A34_H
#define __STATICARRAYINITTYPESIZEU3D24_T0186834C621050282F5EF637E836A6E1DC934A34_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// <PrivateImplementationDetails>___StaticArrayInitTypeSizeU3D24
struct  __StaticArrayInitTypeSizeU3D24_t0186834C621050282F5EF637E836A6E1DC934A34 
{
public:
	union
	{
		struct
		{
			union
			{
			};
		};
		uint8_t __StaticArrayInitTypeSizeU3D24_t0186834C621050282F5EF637E836A6E1DC934A34__padding[24];
	};

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // __STATICARRAYINITTYPESIZEU3D24_T0186834C621050282F5EF637E836A6E1DC934A34_H
#ifndef APPLICATIONEXCEPTION_T664823C3E0D3E1E7C7FA1C0DB4E19E98E9811C74_H
#define APPLICATIONEXCEPTION_T664823C3E0D3E1E7C7FA1C0DB4E19E98E9811C74_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.ApplicationException
struct  ApplicationException_t664823C3E0D3E1E7C7FA1C0DB4E19E98E9811C74  : public Exception_t
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // APPLICATIONEXCEPTION_T664823C3E0D3E1E7C7FA1C0DB4E19E98E9811C74_H
#ifndef ENUM_T2AF27C02B8653AE29442467390005ABC74D8F521_H
#define ENUM_T2AF27C02B8653AE29442467390005ABC74D8F521_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Enum
struct  Enum_t2AF27C02B8653AE29442467390005ABC74D8F521  : public ValueType_t4D0C27076F7C36E76190FB3328E232BCB1CD1FFF
{
public:

public:
};

struct Enum_t2AF27C02B8653AE29442467390005ABC74D8F521_StaticFields
{
public:
	// System.Char[] System.Enum::enumSeperatorCharArray
	CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* ___enumSeperatorCharArray_0;

public:
	inline static int32_t get_offset_of_enumSeperatorCharArray_0() { return static_cast<int32_t>(offsetof(Enum_t2AF27C02B8653AE29442467390005ABC74D8F521_StaticFields, ___enumSeperatorCharArray_0)); }
	inline CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* get_enumSeperatorCharArray_0() const { return ___enumSeperatorCharArray_0; }
	inline CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2** get_address_of_enumSeperatorCharArray_0() { return &___enumSeperatorCharArray_0; }
	inline void set_enumSeperatorCharArray_0(CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* value)
	{
		___enumSeperatorCharArray_0 = value;
		Il2CppCodeGenWriteBarrier((&___enumSeperatorCharArray_0), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
// Native definition for P/Invoke marshalling of System.Enum
struct Enum_t2AF27C02B8653AE29442467390005ABC74D8F521_marshaled_pinvoke
{
};
// Native definition for COM marshalling of System.Enum
struct Enum_t2AF27C02B8653AE29442467390005ABC74D8F521_marshaled_com
{
};
#endif // ENUM_T2AF27C02B8653AE29442467390005ABC74D8F521_H
#ifndef INTPTR_T_H
#define INTPTR_T_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.IntPtr
struct  IntPtr_t 
{
public:
	// System.Void* System.IntPtr::m_value
	void* ___m_value_0;

public:
	inline static int32_t get_offset_of_m_value_0() { return static_cast<int32_t>(offsetof(IntPtr_t, ___m_value_0)); }
	inline void* get_m_value_0() const { return ___m_value_0; }
	inline void** get_address_of_m_value_0() { return &___m_value_0; }
	inline void set_m_value_0(void* value)
	{
		___m_value_0 = value;
	}
};

struct IntPtr_t_StaticFields
{
public:
	// System.IntPtr System.IntPtr::Zero
	intptr_t ___Zero_1;

public:
	inline static int32_t get_offset_of_Zero_1() { return static_cast<int32_t>(offsetof(IntPtr_t_StaticFields, ___Zero_1)); }
	inline intptr_t get_Zero_1() const { return ___Zero_1; }
	inline intptr_t* get_address_of_Zero_1() { return &___Zero_1; }
	inline void set_Zero_1(intptr_t value)
	{
		___Zero_1 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // INTPTR_T_H
#ifndef U3CPRIVATEIMPLEMENTATIONDETAILSU3E_T4846F3E0DC61BBD6AE249FF1EAAC4CB64097DE4A_H
#define U3CPRIVATEIMPLEMENTATIONDETAILSU3E_T4846F3E0DC61BBD6AE249FF1EAAC4CB64097DE4A_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// <PrivateImplementationDetails>
struct  U3CPrivateImplementationDetailsU3E_t4846F3E0DC61BBD6AE249FF1EAAC4CB64097DE4A  : public RuntimeObject
{
public:

public:
};

struct U3CPrivateImplementationDetailsU3E_t4846F3E0DC61BBD6AE249FF1EAAC4CB64097DE4A_StaticFields
{
public:
	// <PrivateImplementationDetails>___StaticArrayInitTypeSizeU3D24 <PrivateImplementationDetails>::1B287FBF8016A1357E0FA5B7B88403B0659ECE97
	__StaticArrayInitTypeSizeU3D24_t0186834C621050282F5EF637E836A6E1DC934A34  ___1B287FBF8016A1357E0FA5B7B88403B0659ECE97_0;
	// <PrivateImplementationDetails>___StaticArrayInitTypeSizeU3D16 <PrivateImplementationDetails>::43223FB64B3397CD2060483DA6EA6B88ADA1734B
	__StaticArrayInitTypeSizeU3D16_t6EB025C40E80FDD74132718092D59E92446BD13D  ___43223FB64B3397CD2060483DA6EA6B88ADA1734B_1;
	// <PrivateImplementationDetails>___StaticArrayInitTypeSizeU3D100 <PrivateImplementationDetails>::F38BC1AFFF2AE826E88401947395497C0C9B2A3B
	__StaticArrayInitTypeSizeU3D100_t8DB8E840BBC5746CC367785F67BA5F3C1589AA9D  ___F38BC1AFFF2AE826E88401947395497C0C9B2A3B_2;

public:
	inline static int32_t get_offset_of_U31B287FBF8016A1357E0FA5B7B88403B0659ECE97_0() { return static_cast<int32_t>(offsetof(U3CPrivateImplementationDetailsU3E_t4846F3E0DC61BBD6AE249FF1EAAC4CB64097DE4A_StaticFields, ___1B287FBF8016A1357E0FA5B7B88403B0659ECE97_0)); }
	inline __StaticArrayInitTypeSizeU3D24_t0186834C621050282F5EF637E836A6E1DC934A34  get_U31B287FBF8016A1357E0FA5B7B88403B0659ECE97_0() const { return ___1B287FBF8016A1357E0FA5B7B88403B0659ECE97_0; }
	inline __StaticArrayInitTypeSizeU3D24_t0186834C621050282F5EF637E836A6E1DC934A34 * get_address_of_U31B287FBF8016A1357E0FA5B7B88403B0659ECE97_0() { return &___1B287FBF8016A1357E0FA5B7B88403B0659ECE97_0; }
	inline void set_U31B287FBF8016A1357E0FA5B7B88403B0659ECE97_0(__StaticArrayInitTypeSizeU3D24_t0186834C621050282F5EF637E836A6E1DC934A34  value)
	{
		___1B287FBF8016A1357E0FA5B7B88403B0659ECE97_0 = value;
	}

	inline static int32_t get_offset_of_U343223FB64B3397CD2060483DA6EA6B88ADA1734B_1() { return static_cast<int32_t>(offsetof(U3CPrivateImplementationDetailsU3E_t4846F3E0DC61BBD6AE249FF1EAAC4CB64097DE4A_StaticFields, ___43223FB64B3397CD2060483DA6EA6B88ADA1734B_1)); }
	inline __StaticArrayInitTypeSizeU3D16_t6EB025C40E80FDD74132718092D59E92446BD13D  get_U343223FB64B3397CD2060483DA6EA6B88ADA1734B_1() const { return ___43223FB64B3397CD2060483DA6EA6B88ADA1734B_1; }
	inline __StaticArrayInitTypeSizeU3D16_t6EB025C40E80FDD74132718092D59E92446BD13D * get_address_of_U343223FB64B3397CD2060483DA6EA6B88ADA1734B_1() { return &___43223FB64B3397CD2060483DA6EA6B88ADA1734B_1; }
	inline void set_U343223FB64B3397CD2060483DA6EA6B88ADA1734B_1(__StaticArrayInitTypeSizeU3D16_t6EB025C40E80FDD74132718092D59E92446BD13D  value)
	{
		___43223FB64B3397CD2060483DA6EA6B88ADA1734B_1 = value;
	}

	inline static int32_t get_offset_of_F38BC1AFFF2AE826E88401947395497C0C9B2A3B_2() { return static_cast<int32_t>(offsetof(U3CPrivateImplementationDetailsU3E_t4846F3E0DC61BBD6AE249FF1EAAC4CB64097DE4A_StaticFields, ___F38BC1AFFF2AE826E88401947395497C0C9B2A3B_2)); }
	inline __StaticArrayInitTypeSizeU3D100_t8DB8E840BBC5746CC367785F67BA5F3C1589AA9D  get_F38BC1AFFF2AE826E88401947395497C0C9B2A3B_2() const { return ___F38BC1AFFF2AE826E88401947395497C0C9B2A3B_2; }
	inline __StaticArrayInitTypeSizeU3D100_t8DB8E840BBC5746CC367785F67BA5F3C1589AA9D * get_address_of_F38BC1AFFF2AE826E88401947395497C0C9B2A3B_2() { return &___F38BC1AFFF2AE826E88401947395497C0C9B2A3B_2; }
	inline void set_F38BC1AFFF2AE826E88401947395497C0C9B2A3B_2(__StaticArrayInitTypeSizeU3D100_t8DB8E840BBC5746CC367785F67BA5F3C1589AA9D  value)
	{
		___F38BC1AFFF2AE826E88401947395497C0C9B2A3B_2 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // U3CPRIVATEIMPLEMENTATIONDETAILSU3E_T4846F3E0DC61BBD6AE249FF1EAAC4CB64097DE4A_H
#ifndef BOUNDSCREATIONEXCEPTION_T48188DFB6D1C45CCF182E9912AE8710B1EE81827_H
#define BOUNDSCREATIONEXCEPTION_T48188DFB6D1C45CCF182E9912AE8710B1EE81827_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// HoloGroup.Geometry.Exceptions.BoundsCreationException
struct  BoundsCreationException_t48188DFB6D1C45CCF182E9912AE8710B1EE81827  : public ApplicationException_t664823C3E0D3E1E7C7FA1C0DB4E19E98E9811C74
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // BOUNDSCREATIONEXCEPTION_T48188DFB6D1C45CCF182E9912AE8710B1EE81827_H
#ifndef KEYCODE_TC93EA87C5A6901160B583ADFCD3EF6726570DC3C_H
#define KEYCODE_TC93EA87C5A6901160B583ADFCD3EF6726570DC3C_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// UnityEngine.KeyCode
struct  KeyCode_tC93EA87C5A6901160B583ADFCD3EF6726570DC3C 
{
public:
	// System.Int32 UnityEngine.KeyCode::value__
	int32_t ___value___2;

public:
	inline static int32_t get_offset_of_value___2() { return static_cast<int32_t>(offsetof(KeyCode_tC93EA87C5A6901160B583ADFCD3EF6726570DC3C, ___value___2)); }
	inline int32_t get_value___2() const { return ___value___2; }
	inline int32_t* get_address_of_value___2() { return &___value___2; }
	inline void set_value___2(int32_t value)
	{
		___value___2 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // KEYCODE_TC93EA87C5A6901160B583ADFCD3EF6726570DC3C_H
#ifndef OBJECT_TAE11E5E46CD5C37C9F3E8950C00CD8B45666A2D0_H
#define OBJECT_TAE11E5E46CD5C37C9F3E8950C00CD8B45666A2D0_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// UnityEngine.Object
struct  Object_tAE11E5E46CD5C37C9F3E8950C00CD8B45666A2D0  : public RuntimeObject
{
public:
	// System.IntPtr UnityEngine.Object::m_CachedPtr
	intptr_t ___m_CachedPtr_0;

public:
	inline static int32_t get_offset_of_m_CachedPtr_0() { return static_cast<int32_t>(offsetof(Object_tAE11E5E46CD5C37C9F3E8950C00CD8B45666A2D0, ___m_CachedPtr_0)); }
	inline intptr_t get_m_CachedPtr_0() const { return ___m_CachedPtr_0; }
	inline intptr_t* get_address_of_m_CachedPtr_0() { return &___m_CachedPtr_0; }
	inline void set_m_CachedPtr_0(intptr_t value)
	{
		___m_CachedPtr_0 = value;
	}
};

struct Object_tAE11E5E46CD5C37C9F3E8950C00CD8B45666A2D0_StaticFields
{
public:
	// System.Int32 UnityEngine.Object::OffsetOfInstanceIDInCPlusPlusObject
	int32_t ___OffsetOfInstanceIDInCPlusPlusObject_1;

public:
	inline static int32_t get_offset_of_OffsetOfInstanceIDInCPlusPlusObject_1() { return static_cast<int32_t>(offsetof(Object_tAE11E5E46CD5C37C9F3E8950C00CD8B45666A2D0_StaticFields, ___OffsetOfInstanceIDInCPlusPlusObject_1)); }
	inline int32_t get_OffsetOfInstanceIDInCPlusPlusObject_1() const { return ___OffsetOfInstanceIDInCPlusPlusObject_1; }
	inline int32_t* get_address_of_OffsetOfInstanceIDInCPlusPlusObject_1() { return &___OffsetOfInstanceIDInCPlusPlusObject_1; }
	inline void set_OffsetOfInstanceIDInCPlusPlusObject_1(int32_t value)
	{
		___OffsetOfInstanceIDInCPlusPlusObject_1 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
// Native definition for P/Invoke marshalling of UnityEngine.Object
struct Object_tAE11E5E46CD5C37C9F3E8950C00CD8B45666A2D0_marshaled_pinvoke
{
	intptr_t ___m_CachedPtr_0;
};
// Native definition for COM marshalling of UnityEngine.Object
struct Object_tAE11E5E46CD5C37C9F3E8950C00CD8B45666A2D0_marshaled_com
{
	intptr_t ___m_CachedPtr_0;
};
#endif // OBJECT_TAE11E5E46CD5C37C9F3E8950C00CD8B45666A2D0_H
#ifndef COMPONENT_T05064EF382ABCAF4B8C94F8A350EA85184C26621_H
#define COMPONENT_T05064EF382ABCAF4B8C94F8A350EA85184C26621_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// UnityEngine.Component
struct  Component_t05064EF382ABCAF4B8C94F8A350EA85184C26621  : public Object_tAE11E5E46CD5C37C9F3E8950C00CD8B45666A2D0
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // COMPONENT_T05064EF382ABCAF4B8C94F8A350EA85184C26621_H
#ifndef BEHAVIOUR_TBDC7E9C3C898AD8348891B82D3E345801D920CA8_H
#define BEHAVIOUR_TBDC7E9C3C898AD8348891B82D3E345801D920CA8_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// UnityEngine.Behaviour
struct  Behaviour_tBDC7E9C3C898AD8348891B82D3E345801D920CA8  : public Component_t05064EF382ABCAF4B8C94F8A350EA85184C26621
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // BEHAVIOUR_TBDC7E9C3C898AD8348891B82D3E345801D920CA8_H
#ifndef MONOBEHAVIOUR_T4A60845CF505405AF8BE8C61CC07F75CADEF6429_H
#define MONOBEHAVIOUR_T4A60845CF505405AF8BE8C61CC07F75CADEF6429_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// UnityEngine.MonoBehaviour
struct  MonoBehaviour_t4A60845CF505405AF8BE8C61CC07F75CADEF6429  : public Behaviour_tBDC7E9C3C898AD8348891B82D3E345801D920CA8
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // MONOBEHAVIOUR_T4A60845CF505405AF8BE8C61CC07F75CADEF6429_H
#ifndef CAMERACONTROLLER_TD0330CCACE4A2E0D05078861257DECB8B8D4F882_H
#define CAMERACONTROLLER_TD0330CCACE4A2E0D05078861257DECB8B8D4F882_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// HoloGroup.Control.CameraController
struct  CameraController_tD0330CCACE4A2E0D05078861257DECB8B8D4F882  : public MonoBehaviour_t4A60845CF505405AF8BE8C61CC07F75CADEF6429
{
public:
	// UnityEngine.KeyCode HoloGroup.Control.CameraController::_catchKey
	int32_t ____catchKey_4;
	// System.Single HoloGroup.Control.CameraController::_movementSpeed
	float ____movementSpeed_5;
	// System.Single HoloGroup.Control.CameraController::_rotationSpeed
	float ____rotationSpeed_6;

public:
	inline static int32_t get_offset_of__catchKey_4() { return static_cast<int32_t>(offsetof(CameraController_tD0330CCACE4A2E0D05078861257DECB8B8D4F882, ____catchKey_4)); }
	inline int32_t get__catchKey_4() const { return ____catchKey_4; }
	inline int32_t* get_address_of__catchKey_4() { return &____catchKey_4; }
	inline void set__catchKey_4(int32_t value)
	{
		____catchKey_4 = value;
	}

	inline static int32_t get_offset_of__movementSpeed_5() { return static_cast<int32_t>(offsetof(CameraController_tD0330CCACE4A2E0D05078861257DECB8B8D4F882, ____movementSpeed_5)); }
	inline float get__movementSpeed_5() const { return ____movementSpeed_5; }
	inline float* get_address_of__movementSpeed_5() { return &____movementSpeed_5; }
	inline void set__movementSpeed_5(float value)
	{
		____movementSpeed_5 = value;
	}

	inline static int32_t get_offset_of__rotationSpeed_6() { return static_cast<int32_t>(offsetof(CameraController_tD0330CCACE4A2E0D05078861257DECB8B8D4F882, ____rotationSpeed_6)); }
	inline float get__rotationSpeed_6() const { return ____rotationSpeed_6; }
	inline float* get_address_of__rotationSpeed_6() { return &____rotationSpeed_6; }
	inline void set__rotationSpeed_6(float value)
	{
		____rotationSpeed_6 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // CAMERACONTROLLER_TD0330CCACE4A2E0D05078861257DECB8B8D4F882_H
#ifndef TRANSFORMMATCHER_TA6D899FE748CA020076F2A79EE5D247BE1BDFD19_H
#define TRANSFORMMATCHER_TA6D899FE748CA020076F2A79EE5D247BE1BDFD19_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// HoloGroup.Geometry.TransformMatcher
struct  TransformMatcher_tA6D899FE748CA020076F2A79EE5D247BE1BDFD19  : public MonoBehaviour_t4A60845CF505405AF8BE8C61CC07F75CADEF6429
{
public:
	// UnityEngine.Transform HoloGroup.Geometry.TransformMatcher::_target
	Transform_tBB9E78A2766C3C83599A8F66EDE7D1FCAFC66EDA * ____target_4;
	// System.Boolean HoloGroup.Geometry.TransformMatcher::_matchPosition
	bool ____matchPosition_5;
	// System.Boolean HoloGroup.Geometry.TransformMatcher::_matchRotation
	bool ____matchRotation_6;
	// System.Boolean HoloGroup.Geometry.TransformMatcher::_matchScale
	bool ____matchScale_7;

public:
	inline static int32_t get_offset_of__target_4() { return static_cast<int32_t>(offsetof(TransformMatcher_tA6D899FE748CA020076F2A79EE5D247BE1BDFD19, ____target_4)); }
	inline Transform_tBB9E78A2766C3C83599A8F66EDE7D1FCAFC66EDA * get__target_4() const { return ____target_4; }
	inline Transform_tBB9E78A2766C3C83599A8F66EDE7D1FCAFC66EDA ** get_address_of__target_4() { return &____target_4; }
	inline void set__target_4(Transform_tBB9E78A2766C3C83599A8F66EDE7D1FCAFC66EDA * value)
	{
		____target_4 = value;
		Il2CppCodeGenWriteBarrier((&____target_4), value);
	}

	inline static int32_t get_offset_of__matchPosition_5() { return static_cast<int32_t>(offsetof(TransformMatcher_tA6D899FE748CA020076F2A79EE5D247BE1BDFD19, ____matchPosition_5)); }
	inline bool get__matchPosition_5() const { return ____matchPosition_5; }
	inline bool* get_address_of__matchPosition_5() { return &____matchPosition_5; }
	inline void set__matchPosition_5(bool value)
	{
		____matchPosition_5 = value;
	}

	inline static int32_t get_offset_of__matchRotation_6() { return static_cast<int32_t>(offsetof(TransformMatcher_tA6D899FE748CA020076F2A79EE5D247BE1BDFD19, ____matchRotation_6)); }
	inline bool get__matchRotation_6() const { return ____matchRotation_6; }
	inline bool* get_address_of__matchRotation_6() { return &____matchRotation_6; }
	inline void set__matchRotation_6(bool value)
	{
		____matchRotation_6 = value;
	}

	inline static int32_t get_offset_of__matchScale_7() { return static_cast<int32_t>(offsetof(TransformMatcher_tA6D899FE748CA020076F2A79EE5D247BE1BDFD19, ____matchScale_7)); }
	inline bool get__matchScale_7() const { return ____matchScale_7; }
	inline bool* get_address_of__matchScale_7() { return &____matchScale_7; }
	inline void set__matchScale_7(bool value)
	{
		____matchScale_7 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // TRANSFORMMATCHER_TA6D899FE748CA020076F2A79EE5D247BE1BDFD19_H





#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize4600 = { sizeof (TransformMatcher_tA6D899FE748CA020076F2A79EE5D247BE1BDFD19), -1, 0, 0 };
extern const int32_t g_FieldOffsetTable4600[4] = 
{
	TransformMatcher_tA6D899FE748CA020076F2A79EE5D247BE1BDFD19::get_offset_of__target_4(),
	TransformMatcher_tA6D899FE748CA020076F2A79EE5D247BE1BDFD19::get_offset_of__matchPosition_5(),
	TransformMatcher_tA6D899FE748CA020076F2A79EE5D247BE1BDFD19::get_offset_of__matchRotation_6(),
	TransformMatcher_tA6D899FE748CA020076F2A79EE5D247BE1BDFD19::get_offset_of__matchScale_7(),
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize4601 = { sizeof (BoundsExtension_t0CDA9EFB63602DAF0A2FF8702DD9143A062CB613), -1, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize4602 = { sizeof (MatrixExtensions_t367F29DBDD3C67BA169CA1B8677E181756ABA5D1), -1, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize4603 = { sizeof (BoundsCreationException_t48188DFB6D1C45CCF182E9912AE8710B1EE81827), -1, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize4604 = { sizeof (CameraController_tD0330CCACE4A2E0D05078861257DECB8B8D4F882), -1, 0, 0 };
extern const int32_t g_FieldOffsetTable4604[3] = 
{
	CameraController_tD0330CCACE4A2E0D05078861257DECB8B8D4F882::get_offset_of__catchKey_4(),
	CameraController_tD0330CCACE4A2E0D05078861257DECB8B8D4F882::get_offset_of__movementSpeed_5(),
	CameraController_tD0330CCACE4A2E0D05078861257DECB8B8D4F882::get_offset_of__rotationSpeed_6(),
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize4605 = { sizeof (U3CPrivateImplementationDetailsU3E_t4846F3E0DC61BBD6AE249FF1EAAC4CB64097DE4A), -1, sizeof(U3CPrivateImplementationDetailsU3E_t4846F3E0DC61BBD6AE249FF1EAAC4CB64097DE4A_StaticFields), 0 };
extern const int32_t g_FieldOffsetTable4605[3] = 
{
	U3CPrivateImplementationDetailsU3E_t4846F3E0DC61BBD6AE249FF1EAAC4CB64097DE4A_StaticFields::get_offset_of_U31B287FBF8016A1357E0FA5B7B88403B0659ECE97_0(),
	U3CPrivateImplementationDetailsU3E_t4846F3E0DC61BBD6AE249FF1EAAC4CB64097DE4A_StaticFields::get_offset_of_U343223FB64B3397CD2060483DA6EA6B88ADA1734B_1(),
	U3CPrivateImplementationDetailsU3E_t4846F3E0DC61BBD6AE249FF1EAAC4CB64097DE4A_StaticFields::get_offset_of_F38BC1AFFF2AE826E88401947395497C0C9B2A3B_2(),
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize4606 = { sizeof (__StaticArrayInitTypeSizeU3D16_t6EB025C40E80FDD74132718092D59E92446BD13D)+ sizeof (RuntimeObject), sizeof(__StaticArrayInitTypeSizeU3D16_t6EB025C40E80FDD74132718092D59E92446BD13D ), 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize4607 = { sizeof (__StaticArrayInitTypeSizeU3D24_t0186834C621050282F5EF637E836A6E1DC934A34)+ sizeof (RuntimeObject), sizeof(__StaticArrayInitTypeSizeU3D24_t0186834C621050282F5EF637E836A6E1DC934A34 ), 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize4608 = { sizeof (__StaticArrayInitTypeSizeU3D100_t8DB8E840BBC5746CC367785F67BA5F3C1589AA9D)+ sizeof (RuntimeObject), sizeof(__StaticArrayInitTypeSizeU3D100_t8DB8E840BBC5746CC367785F67BA5F3C1589AA9D ), 0, 0 };
#ifdef __clang__
#pragma clang diagnostic pop
#endif
