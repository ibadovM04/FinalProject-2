//using Gma.QrCodeNet.Encoding;
//using QRCoder;
//using SkiaSharp;
//using System.Reflection.Emit;

//namespace FinalProject.QrCode;
// public class QrCodeService
// {
//    public string GenerateQrCode(string content)
//    {
//        using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
//        {
//            QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
//            using (QRCode qrCode = new QRCode(qrCodeData))
//            {
//                using (SKBitmap qrCodeImage = qrCode.GetGraphic(20))
//                {
//                    using (SKImage image = SKImage.FromBitmap(qrCodeImage))
//                    {
//                        using (SKData data = image.Encode(SKEncodedImageFormat.Png, 100))
//                        {
//                            return Convert.ToBase64String(data.ToArray());
//                        }
//                    }
//                }
//            }
//        }
//    }
//}


