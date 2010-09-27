using System.Windows.Media;
using HLGranite.Jawi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Shapes;

namespace HLGranite.Jawi.Test
{
    /// <summary>
    /// This is a test class for WorkspaceTest and is intended
    /// to contain all WorkspaceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class WorkspaceTest
    {
        /// <summary>
        /// A test for GetLabel
        /// </summary>
        [TestMethod()]
        public void GetLabelTest()
        {
            Workspace_Accessor target = new Workspace_Accessor(@"E:\jawi\khots");
            string[] testers = new string[] { "file.svg", "file2.svg", "file3.svg", "nor.svg", "nor2.svg", "noor.svg", "noor2.svg", "noor21.svg", };
            string[] expecteds = new string[] { string.Empty, "2", "3", string.Empty, "2", string.Empty, "2", "21", };
            for (int i = 0; i < testers.Length; i++)
            {
                string expected = expecteds[i];
                string actual = actual = target.GetLabel(testers[i]);
                Assert.AreEqual(expected, actual);
            }
        }
        /// <summary>
        /// A test for loading khot library.
        /// </summary>
        [TestMethod()]
        public void InitializeTest()
        {
            Workspace target = new Workspace(@"E:\jawi\khots");
            Assert.IsTrue(target.Items.Count > 0);
        }
        /// <summary>
        ///A test for Select
        ///</summary>
        [TestMethod()]
        public void SelectTest()
        {
            Workspace target = new Workspace(@"E:\jawi\khots");
            //alef raw data
            string alef = "M2.8853219,14.535578L3.4452919,24.413808 6.0826619,72.682508C6.0826619,72.682508 7.8725619,70.888858 8.9450019,69.255188 10.466172,66.940318 10.546172,64.950418 11.003652,62.728038 11.362382,60.984378 11.386132,60.595648 11.238632,58.834488L8.3512919,24.397558 8.0763019,18.551618C8.3637919,18.709108,8.6375219,18.851598,8.8950119,18.977838L13.337272,21.177728 12.489822,18.840348C12.393622,18.575368 12.291082,18.294128 12.188582,18.011638 10.956152,14.616818 10.272432,11.259498 9.4512319,7.77842770000001 8.6662719,4.44610770000001 7.6188219,0.00758770000001263 7.6188219,0.00758770000001263 7.6188219,0.00758770000001263 6.8776119,-0.0311122999999874 6.1939019,0.0662877000000126 4.3165019,0.335027700000013 0.307961849999997,6.31470770000001 0.0479718499999979,7.67338770000001 0.0229718499999979,7.80212770000001 0.00797184999999793,7.93087770000001 0.00297184999999793,8.05836770000001 -0.00832815000000207,8.33959770000001 0.0129718499999979,8.63458770000001 0.0604718499999979,8.93956770000001 0.404201849999998,11.129458 1.5066519,12.998108 2.8853219,14.535528z";
            Path path = new Path();
            path.Data = (Geometry)new GeometryConverter().ConvertFromString(alef);

            PathViewModel viewModel = new PathViewModel(string.Empty, path, string.Empty);
            target.Select(viewModel);
            //check SelectedPath
            Assert.AreEqual(target.SelectedPath.Path.Data.ToString(),viewModel.Path.Data.ToString());

            //ensure always only one Item IsChecked.
            int count = 0;
            foreach (PathViewModel item in target.Items)
                if (item.IsChecked) count++;
            Assert.AreEqual(1, count);

            //select next object.
            string dot = "M3.1054696,15.75434C3.6094426,18.82793 4.5492686,23.76517 4.0869176,25.03635 3.7460606,25.97381 3.0559716,26.66627 2.4212556,27.49747 -0.2264804,30.96604 -0.6113344,35.03333 0.8220896,39.70933 2.3521336,44.70032 7.7263506,42.62043 12.616468,39.51934 15.53069,37.67069 22.65219,32.61096 24.193108,29.29988 24.278228,29.11614 24.35785,28.92865 24.432221,28.73616 23.762131,26.18129 21.683116,28.80496 21.676741,28.80866 13.716785,34.24837 5.3694746,37.5407 2.7644876,36.5195 2.1845176,36.29201 1.8942836,35.68829 2.2572636,34.40961 2.4872516,33.5984 2.9583516,32.7547 3.0645966,32.6272 4.2296596,31.21978 6.6485326,29.7861 8.1425786,28.15369 9.0814046,27.12749 9.6549996,26.02255 10.645197,25.32509 11.898881,24.44264 13.820405,24.21265 15.293827,23.55393 19.110751,21.84777 22.053346,17.22927 22.415827,13.61696 23.065668,7.1410503 19.6006,5.0074103 13.9169,1.0438703 13.15794,0.5139003 12.546472,0.0951703 11.285414,0.0151703000000001 9.9611076,-0.0685296999999999 7.9205906,0.2214103 7.9205906,0.2214103 7.9205906,0.2214103 7.8168456,3.3425003 8.2474486,5.5648803 8.7287976,8.0485003 9.8782376,9.4096803 10.368087,11.07084 10.477081,11.44082 10.553452,11.8258 10.422457,11.95954 10.287464,12.09704 9.9326076,11.96854 9.5935006,11.75455 6.9287656,10.06839 6.7901486,7.6610203 4.6721346,8.7434603 3.5073216,9.3396803 2.9713496,10.73336 2.8809796,12.64201 2.8233596,13.85944 2.8458596,14.17068 3.1054676,15.75434z M12.059248,8.3897303L11.909756,7.1123003C11.909756,7.1123003 12.745087,6.9660603 13.331431,7.0248003 14.077891,7.1010003 14.420623,7.5122803 14.86935,7.9035103 17.496086,10.19839 20.907657,11.09834 19.429234,13.39572 19.191497,13.76445 17.71095,15.29812 16.400519,16.42056 14.615988,17.94923 13.518921,18.30171 13.693787,16.38806 13.965647,13.41697 11.998626,7.8710103 12.059248,8.3897303z M9.0800296,19.17791L7.4994876,19.27421C6.7894006,19.31791 6.6677816,19.30921 6.2840516,19.19541 5.5918386,18.99042 4.7182596,18.27046 4.3830266,17.58425 4.2211606,17.25301 4.1994116,17.15302 4.1646636,16.5843L4.0872936,15.32062 5.3517266,15.24312C5.9198216,15.20812 6.0216916,15.21812 6.3701736,15.33812 7.3446216,15.67435 8.8960406,17.33176 8.9930346,17.75549 9.1320276,18.36296 9.0800346,19.17791 9.0800346,19.17791z";
            Path path2 = new Path();
            path2.Data = (Geometry)new GeometryConverter().ConvertFromString(dot);
            PathViewModel viewModel2 = new PathViewModel(string.Empty, path2, string.Empty);
            target.Select(viewModel2);
            //check SelectedPath
            Assert.AreEqual(target.SelectedPath.Path.Data.ToString(), path2.Data.ToString());

            //ensure always only one Item IsChecked.
            count = 0;
            foreach (PathViewModel item in target.Items)
                if (item.IsChecked) count++;
            Assert.AreEqual(1, count);
        }
    }
}