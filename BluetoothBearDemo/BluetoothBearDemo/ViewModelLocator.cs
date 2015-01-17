using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace BluetoothBearDemo
{
    public class ViewModelLocator
    {
        public const string AddCommentPageKey = "AddCommentPage";
        public const string DetailsPageKey = "DetailsPage";

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "This non-static member is needed for data binding purposes.")]

        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<ScanViewModel>();


        }



        public ScanViewModel ScanVM
        {
            get
            {
                try
                {
                    return ServiceLocator.Current.GetInstance<ScanViewModel>();
                }
                catch (TargetInvocationException ex)
                {
                    string s = ex.ToString();
                    Debug.WriteLine(s);
                    return null;
                }
            }
        }
    }


}
