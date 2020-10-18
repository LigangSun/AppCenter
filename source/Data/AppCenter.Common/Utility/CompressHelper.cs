using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;

namespace SoonLearning.AppCenter.Utility
{
    public class Compression
    {
        #region Const

        private const int buffer_size = 1024;

        #endregion

        #region Compress

        public static void CompressFile(string sourceFile, string destinationFile)
        {
            // make sure the source file is there
            if (File.Exists(sourceFile) == false)
                throw new FileNotFoundException();

            // Create the streams and byte arrays needed
            FileStream sourceStream = null;

            try
            {
                // Read the bytes from the source file into a byte array
                sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read);

                CompressStream(sourceStream, destinationFile);
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }
            finally
            {
                // Make sure we allways close all streams
                if (sourceStream != null)
                    sourceStream.Close();
            }
        }

        public static void CompressStream(Stream sourceStream, string destinationFile)
        {
            FileStream destinationStream = null;

            try
            {
                // Open the FileStream to write to
                destinationStream = new FileStream(destinationFile, FileMode.OpenOrCreate, FileAccess.Write);

                CompressStream(sourceStream, destinationStream);
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }
            finally
            {
                if (destinationStream != null)
                    destinationStream.Close();
            }
        }

        public static void CompressStream(Stream sourceStream, Stream destinationStream)
        {
            GZipStream compressedStream = null;
            try
            {
                // Read the source stream values into the buffer
                byte[] buffer = new byte[sourceStream.Length];
                int checkCounter = sourceStream.Read(buffer, 0, buffer.Length);

                if (checkCounter != buffer.Length)
                {
                    throw new ApplicationException();
                }

                // Create a compression stream pointing to the destiantion stream
                compressedStream = new GZipStream(destinationStream, CompressionMode.Compress, true);

                // Now write the compressed data to the destination file
                compressedStream.Write(buffer, 0, buffer.Length);

                compressedStream.Flush();
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }
            finally
            {
                if (compressedStream != null)
                    compressedStream.Close();
            }
        }

        public static byte[] CompressBuffer(byte[] buffer)
        {
            GZipStream compressedStream = null;
            MemoryStream ms = new MemoryStream();
            try
            {
                // Create a compression stream pointing to the destiantion stream
                compressedStream = new GZipStream(ms, CompressionMode.Compress, true);

                // Now write the compressed data to the destination file
                compressedStream.Write(buffer, 0, buffer.Length);

                compressedStream.Close();
                ms.Flush();

                return ms.ToArray();
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
                throw ex;
            }
            finally
            {
                if (compressedStream != null)
                    compressedStream.Close();

                if (ms != null)
                    ms.Close();
            }
        }

        #endregion

        #region Decompress

        public static void DecompressFile(string sourceFile, string destinationFile)
        {
            // make sure the source file is there
            if (File.Exists(sourceFile) == false)
                throw new FileNotFoundException();

            // Create the streams and byte arrays needed
            FileStream sourceStream = null;

            try
            {
                // Read in the compressed source stream
                sourceStream = new FileStream(sourceFile, FileMode.Open);
                DecompressStream(sourceStream, destinationFile);
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }
            finally
            {
                // Make sure we allways close all streams
                if (sourceStream != null)
                    sourceStream.Close();
            }
        }

        public static void DecompressStream(Stream sourceStream, string destinationFile)
        {
            FileStream destinationStream = null;
            try
            {
                // Now write everything to the destination file
                destinationStream = new FileStream(destinationFile, FileMode.Create);
                DecompressStream(sourceStream, destinationStream);
            }
            catch (System.Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }
            finally
            {
                if (destinationStream != null)
                    destinationStream.Close();
            }
        }

        public static void DecompressStream(Stream sourceStream, Stream destinationStream)
        {
            GZipStream decompressedStream = null;

            try
            {
                // Create a compression stream pointing to the destiantion stream
                decompressedStream = new GZipStream(sourceStream, CompressionMode.Decompress, true);

                // Read the footer to determine the length of the destiantion file
                byte[] quartetBuffer = new byte[4];
                int position = (int)sourceStream.Length - 4;
                sourceStream.Position = position;
                sourceStream.Read(quartetBuffer, 0, 4);
                sourceStream.Position = 0;
                int checkLength = BitConverter.ToInt32(quartetBuffer, 0);

                MemoryStream ms = new MemoryStream();
                int total = ReadAllBytesFromStream(decompressedStream, ms);

                // Now write everything to the destination file
                destinationStream.Write(ms.ToArray(), 0, total);

                ms.Close();

                // and flush everyhting to clean out the buffer
                destinationStream.Flush();
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }
            finally
            {
                if (decompressedStream != null)
                    decompressedStream.Close();
            }
        }

        public static byte[] DecompressBuffer(byte[] buffer)
        {
            GZipStream decompressedStream = null;

            try
            {
                MemoryStream sourceStream = new MemoryStream(buffer);
                // Create a compression stream pointing to the destiantion stream
                decompressedStream = new GZipStream(sourceStream, CompressionMode.Decompress, true);

                MemoryStream destStream = new MemoryStream();

                int total = ReadAllBytesFromStream(decompressedStream, destStream);

                byte[] destBuf = destStream.ToArray();

                destStream.Close();

                return destBuf;
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
                throw ex;
            }
            finally
            {
                if (decompressedStream != null)
                    decompressedStream.Close();
            }
        }

        #endregion

        #region Help Method

        private static int ReadAllBytesFromStream(Stream stream, MemoryStream buffer)
        {
            // Use this method is used to read all bytes from a stream.
            int offset = 0;
            int totalCount = 0;

            while (true)
            {
                byte[] temp = new byte[buffer_size];
                int bytesRead = stream.Read(temp, 0, buffer_size);
                if (bytesRead == 0)
                {
                    break;
                }
                offset += bytesRead;
                totalCount += bytesRead;

                buffer.Write(temp, 0, bytesRead);
            }

            buffer.Flush();
            return totalCount;
        }

        public static bool CompareData(byte[] buf1, int len1, byte[] buf2, int len2)
        {
            // Use this method to compare data from two different buffers.
            if (len1 != len2)
            {
                return false;
            }

            for (int i = 0; i < len1; i++)
            {
                if (buf1[i] != buf2[i])
                {
                    Console.WriteLine("byte {0} is different {1}|{2}", i, buf1[i], buf2[i]);
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}
