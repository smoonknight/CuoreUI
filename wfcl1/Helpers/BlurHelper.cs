using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace CuoreUI.Helpers
{
    public static class Blurs
    {
        private static float Clamp(float value, float min, float max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }

        public static class QuadraticBlur
        {
            public unsafe static void Apply(ref Bitmap bitmap, float radius)
            {
                if (radius < 0.1f)
                    return;

                BitmapData srcData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                int stride = srcData.Stride;
                IntPtr scan0 = srcData.Scan0;

                int width = bitmap.Width;
                int height = bitmap.Height;

                byte* src = (byte*)scan0.ToPointer();

                float[] kernel = CreateQuadraticKernel(radius);
                int kernelSize = kernel.Length;
                int halfKernel = kernelSize / 2;

                byte* temp = stackalloc byte[height * stride];

                // Horizontal pass
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        float b = 0, g = 0, r = 0;
                        float weightSum = 0;

                        for (int k = -halfKernel; k <= halfKernel; k++)
                        {
                            int pixelPos = x + k;
                            if (pixelPos < 0 || pixelPos >= width)
                                continue;

                            byte* p = src + y * stride + pixelPos * 3;

                            float weight = kernel[halfKernel + k];
                            b += p[0] * weight;
                            g += p[1] * weight;
                            r += p[2] * weight;
                            weightSum += weight;
                        }

                        byte* dst = temp + y * stride + x * 3;

                        dst[0] = (byte)Clamp(b / weightSum, 0, 255);
                        dst[1] = (byte)Clamp(g / weightSum, 0, 255);
                        dst[2] = (byte)Clamp(r / weightSum, 0, 255);
                    }
                }

                // Vertical pass
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        float b = 0, g = 0, r = 0;
                        float weightSum = 0;

                        for (int k = -halfKernel; k <= halfKernel; k++)
                        {
                            int pixelPos = y + k;
                            if (pixelPos < 0 || pixelPos >= height)
                                continue;

                            byte* p = temp + pixelPos * stride + x * 3;

                            float weight = kernel[halfKernel + k];
                            b += p[0] * weight;
                            g += p[1] * weight;
                            r += p[2] * weight;
                            weightSum += weight;
                        }

                        byte* dst = src + y * stride + x * 3;

                        dst[0] = (byte)Clamp(b / weightSum, 0, 255);
                        dst[1] = (byte)Clamp(g / weightSum, 0, 255);
                        dst[2] = (byte)Clamp(r / weightSum, 0, 255);
                    }
                }

                bitmap.UnlockBits(srcData);
            }

            private static float[] CreateQuadraticKernel(float radius)
            {
                int size = (int)(Math.Ceiling(radius) * 2) + 1;
                float[] kernel = new float[size];
                float radiusSquared = radius * radius;

                for (int i = 0; i < size; i++)
                {
                    float x = i - radius;
                    kernel[i] = 1 - x * x / radiusSquared;
                    if (kernel[i] < 0)
                        kernel[i] = 0;
                }

                return kernel;
            }
        }
    }
}