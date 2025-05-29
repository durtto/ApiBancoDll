using BoletoOnline.BancoBrasil;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace BoletoOnline.BancoBrasil
{
   [ComVisible(true)]
   [ClassInterface(ClassInterfaceType.None)]
   [Guid("4CF02CB0-9504-46C5-95DC-2D539AD3FFAE")]
   public class TratarBoleto : ITratarBoleto
   {
      public void SaveToFile(string filePath, string text)
      {
         string directoryPath = Path.GetDirectoryName(filePath);
         if (!Directory.Exists(directoryPath))
         {
            Directory.CreateDirectory(directoryPath);
         }
         File.WriteAllText(filePath, text);
      }





    }
}
