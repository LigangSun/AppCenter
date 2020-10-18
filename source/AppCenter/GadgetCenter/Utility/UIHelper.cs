using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.IO;
using System.Windows.Media;
using System.Windows.Controls;
using System.Reflection;
using SoonLearning.AppCenter.Interfaces;

namespace SoonLearning.AppCenter.Utility
{
    public class GC_UIHelper
    {
        public static Vector MatchImageToWnd(Size imgSize, Size wndSize)
        {
            double x;
            double y;
            
            double wndWidth = wndSize.Width;
            double wndHeight = wndSize.Height;
            double ratio = wndWidth / wndHeight;
            double imageRatio = imgSize.Width / imgSize.Height;
            if (imgSize.Width > wndWidth &&
                imgSize.Height > wndHeight)
            {
                if (imageRatio >= ratio)
                {
                    x = wndWidth;
                    y = x / imageRatio;
                }
                else
                {
                    y = wndHeight;
                    x = y * imageRatio;
                }
            }
            else if (imgSize.Width > wndWidth)
            {
                x = wndWidth;
                y = x / imageRatio;
            }
            else if (imgSize.Height > wndHeight)
            {
                y = wndHeight;
                x = y * imageRatio;
            }
            else
            {
                x = imgSize.Width;
                y = imgSize.Height;
            }

            return new Vector(x, y);
        }

        public static byte[] GetBitmapData(string file, int width, int height)
        {
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(file);
            System.Drawing.Image thumbnail = bmp;//bmp.GetThumbnailImage(width, height, null, IntPtr.Zero);

            MemoryStream ms = new MemoryStream();
            thumbnail.Save(ms, bmp.RawFormat);
            byte[] data = ms.ToArray();

            ms.Dispose();

            thumbnail.Dispose();
            bmp.Dispose();

            return data;
        }

        /// <summary> 
        /// Finds a Child of a given item in the visual tree.  
        /// </summary> 
        /// <param name="parent">A direct parent of the queried item.</param> 
        /// <typeparam name="T">The type of the queried item.</typeparam> 
        /// <param name="childName">x:Name or Name of child. </param> 
        /// <returns>The first parent item that matches the submitted type parameter.  
        /// If not matching item can be found,  
        /// a null parent is being returned.</returns> 
        public static T FindChild<T>(DependencyObject parent, string childName)
           where T : DependencyObject
        {
            // Confirm parent and childName are valid.  
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child 
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree 
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child.  
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search 
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name 
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found. 
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }

        public static T FindChild<T>(DependencyObject parent, UIElement childToFind)
           where T : DependencyObject
        {
            // Confirm parent and childName are valid.  
            if (parent == null) return null;

            if (parent == childToFind)
                return (T)parent;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child 
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree 
                    foundChild = FindChild<T>(child, childToFind);

                    // If the child is found, break so we do not overwrite the found child.  
                    if (foundChild != null) break;
                }
                else if (childToFind != null)
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search 
                    if (frameworkElement != null && frameworkElement == childToFind)
                    {
                        // if the child's name is of the request name 
                        foundChild = (T)child;
                        break;
                    }
                    else
                    {
                        // recursively drill down the tree 
                        foundChild = FindChild<T>(frameworkElement, childToFind);

                        // If the child is found, break so we do not overwrite the found child.  
                        if (foundChild != null)
                            break;
                    }
                }
                else
                {
                    // child element found. 
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }

        public static T FindParent<T>(DependencyObject from)
            where T : class
        {
            T result = null;
            DependencyObject parent = VisualTreeHelper.GetParent(from);

            if (parent is T)
                result = parent as T;
            else if (parent != null)
                result = FindParent<T>(parent);

            return result;
        } 

        public static void ShowMessageWindow(System.Windows.Controls.UserControl msgWnd)
        {
            if (App.Current.MainWindow is IMessageControl)
            {
                IMessageControl messageCtrl = App.Current.MainWindow as IMessageControl;
                messageCtrl.ShowMessageWindow(msgWnd);
            }
        }

        public static void CloseMessageWindow()
        {
            if (App.Current.MainWindow is IMessageControl)
            {
                IMessageControl messageCtrl = App.Current.MainWindow as IMessageControl;
                messageCtrl.CloseMessageWindow();
            }
        }

        public static AppDomain CreateAppDomain()
        {
            // Get and display the full name of the EXE assembly.
            string exeAssembly = Assembly.GetEntryAssembly().Location;
            Console.WriteLine(exeAssembly);

            // Construct and initialize settings for a second AppDomain.
            AppDomainSetup ads = new AppDomainSetup();
            ads.ApplicationBase =
                "file:///" + System.Environment.CurrentDirectory;// Path.GetDirectoryName(exeAssembly);
            ads.DisallowBindingRedirects = false;
            ads.DisallowCodeDownload = true;
            ads.ConfigurationFile =
                AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

            // Create the second AppDomain.
            return AppDomain.CreateDomain(Guid.NewGuid().ToString("N"), null, ads);
        }

        internal static IEnumerable<string> GetCurrentUser()
        {
            yield return "版本: " + Assembly.GetEntryAssembly().GetName().Version.ToString();
        }
    }
}
