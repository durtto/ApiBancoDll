using System;
using System.Runtime.InteropServices;

namespace ApiBancos
{
   [ComVisible(true)]
   [InterfaceType(ComInterfaceType.InterfaceIsDual)]
   [Guid("9ABC47F7-AD42-4B4E-8243-B7FC96F0B96B")]
   public interface ITratarBoleto
   {
      void SaveToFile(string filePath, string text);
   }





}
