#if TTP_BILLING && UIAP_INSTALLED && (UNITY_ANDROID || UNITY_IPHONE)
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class TTPRealAppleTangle
    {
        private static byte[] data = System.Convert.FromBase64String("Uz3bijowAnUjTWJ7fihgXnllTWvKZsDuP1lvV7pyYMsw4SMetTb9av1pVq0UOukLdIOJFvBTPduKOjACXRwTGV0eGA8JFBsUHhwJFBITXQ0TGV0eEhMZFAkUEhMOXRIbXQgOGLRkD4ggc6gCIuaPWH7HKPIwIHCM6OMHcdk69iapa0pOtrlyMLNpFKwHTf98C01ze34oYHJ8fIJ5eX5/fFf7NfuKcHx8eHh9TR9Mdk10e34oNKUL4k5pGNwK6bRQf358fXze/3wUGxQeHAkUEhNdPAgJFRIPFAkETE1se34oeXdudzwNDREYXTQTHlNMOANiMRYt6zz0uQkfdm3+PPpO9/xRXR4YDwkUGxQeHAkYXQ0SERQeBC8YERQcEx4YXRITXQkVFA5dHhgPeH1+/3xyfU3/fHd//3x8fZns1HR1I03/fGx7fihgXXn/fHVN/3x5TXt+KGBzeWt5aVatFDrpC3SDiRbwQFsaXfdOF4pw/7Kjlt5ShC4XJhn2ZPSjhDYRiHrWX01/lWVDhS10rg0RGF0vEhIJXT48TWNqcE1LTUlPcHt0V/s1+4pwfHx4eH1+/3x8fSH/fH17dFf7NfuKHhl4fE38j01XewoKUxwNDREYUx4SEFIcDQ0RGB4cXT48Tf98X01we3RX+zX7inB8fHwfERhdDgkcExkcDxldCRgPEA5dHA8cHgkUHhhdDgkcCRgQGBMJDlNNeXtufyguTG5NbHt+KHl3bnc8DQ0EXRwODggQGA5dHB4eGA0JHBMeGAkUGxQeHAkYXR8EXRwTBF0NHA8JCRUSDxQJBExrTWl7fih5fm5wPA1OSydNH0x2TXR7fih5e25/KC5MbkhPTElNTksnanBOSE1PTURPTElNcuBAjlY0VWe1g7PIxHOkI2GrtkANERhdPhgPCRQbFB4cCRQSE108CL0eTgqKR3pRK5anclxzp8cOZDLIdVZ7fHh4en98a2MVCQkNDkdSUgoZSF5oNmgkYM7piovh47Itx7wlLVtNWXt+KHl2bmA8DQ0RGF0+GA8JyEfQiXJzfe92zFxrUwmoQXCmH2sCPNXlhKy3G+FZFmyt3saZZle+YmLspmM6LZZ4kCME+VCWS98qMSiRGvJ1yV2KttFRXRINy0J8TfHKPrLW3gzvOi4ovNJSPM6Fhp4NsJveMcOJDuaTrxlytgQySaXfQ4QFgha1epEARP72Ll2uRbnMwucydxaCVoGkSwK8+iik2uTETz+GpagM4wPcLxEYXTQTHlNMW01Ze34oeXZuYDwNYvj++GbkQDpKj9TmPfNRqcztb6XMTSWRJ3lP8RXO8mCjGA6CGiMYwSTaeHQBaj0rbGMJrsr2XkY63qgSWZ+WrMoNonI4nFq3jBAFkJrIampSTfy+e3VWe3x4eHp/f038y2f8zntNcnt+KGBufHyCeXhNfnx8gk1gXRIbXQkVGF0JFRgTXRwNDREUHhxrTWl7fih5fm5wPA0NERhdLxISCUvkMVAFypDx5qGOCuaPC68KTTK81aEDX0i3WKikcqsWqd9ZXmyK3NHyDvwdu2YmdFLvz4U5NY0dReNoiE3/ecZN/37e3X5/fH9/fH9NcHt0Ldf3qKeZga10ekrNCAhc");
        private static int[] order = new int[] { 31,59,32,45,43,19,18,28,59,27,11,27,25,44,38,38,33,30,56,33,56,47,34,36,44,42,46,32,40,39,33,59,40,34,43,57,57,56,40,57,43,43,47,44,50,51,58,51,49,58,52,53,56,57,56,57,58,58,58,59,60 };
        private static int key = 125;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif