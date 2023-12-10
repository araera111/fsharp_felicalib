open System.Runtime.InteropServices
open System



(* pasoriPointerを使って、pasoriを使う *)


module InteropWithNative =
    open System

    [<DllImport(@"felicalib.dll")>]
    (* 返り値はpasori handleである *)
    extern IntPtr pasori_open()


    [<DllImport(@"felicalib.dll")>]
    extern void pasori_close()

    [<DllImport(@"felicalib.dll")>]
    extern void pasori_init(IntPtr)

    [<DllImport(@"felicalib.dll")>]
    extern IntPtr felica_polling(IntPtr, int, int, int)
    (* 

void felica_getidm	(	felica * 	f,
uint8 * 	buf	 
)	
 *)
    [<DllImport(@"felicalib.dll")>]
    extern void felica_getidm(IntPtr, byte[])




(* pasoriPointer *)
let pasoriPointer = InteropWithNative.pasori_open ()

(* pasori_init *)
InteropWithNative.pasori_init (pasoriPointer)

(* felica_polling *)
let felica_polling = InteropWithNative.felica_polling (pasoriPointer, 0xffff, 0, 0)

(* 
構造体 felica
FeliCa ハンドル [詳細]
#include <felicalib.h>


変数
pasori * 	p
uint16 	systemcode
uint8 	IDm [8]
uint8 	PMm [8]
uint8 	num_system_code
uint16 	system_code [MAX_SYSTEM_CODE]
uint8 	num_area_code
uint16 	area_code [MAX_AREA_CODE]
uint16 	end_service_code [MAX_AREA_CODE]
uint8 	num_service_code
uint16 	service_code [MAX_SERVICE_CODE]
 *)

let byte_array = Array.zeroCreate 8

(* byte to string *)
let byteToStr byteArr = BitConverter.ToString(byteArr)

let f = InteropWithNative.felica_getidm (felica_polling, byte_array)
(* 
010101120c0b8a3b9000
 *)
printfn "IDm: %A" (byteToStr byte_array)

(* log.txtファイルに書き出す *)
let log = System.IO.File.AppendText("log.txt")
log.WriteLine(byteToStr byte_array)
log.Close()
