using lib;
using System.Reflection;

namespace rarbg_sub_renamer_lib_Tests
{
    public class Renamer_Tests
    {
        private Renamer renamer = new Renamer();

        [Fact]
        public void SetBaseDirectory_WhenFolderDoesntExist_ReturnsFalse()
        {
            //arrange
            var fakePath = "INVALID FAKE DIRECTORY PATH";

            //act
            var result = renamer.SetBaseDirectory(fakePath);

            //assert
            Assert.False(result);
        }

        [Fact]
        public void SetBaseDirectory_WhenFolderDoesExistButNameDoesntEndWithSubs_ReturnsFalse()
        {
            // arrange
            // path where current executing assembly is
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // act
            var result = renamer.SetBaseDirectory(path!);

            // assert
            Assert.False(result);

        }

        [Fact]
        public void SetOutputDirectory_IfBaseDirNotSet_DoesNothing()
        {
            // arrange
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
                "/TestFolderCreated");
            //delete test folder if already exists
            if (Directory.Exists(path)) Directory.Delete(path);

            // act
            renamer.SetOutputDirectory(path);
            var result = Directory.Exists(path);

            // assert
            Assert.False(result);

        }

        [Fact]
        public void SetOutputDirectory_IfBaseDirSet_CreatesOutputFolder()
        {
            // arrange
            var baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
            var path = Path.Combine(baseDir, "/TestFolderCreated");
            //delete test folder if already exists
            if (Directory.Exists(path)) Directory.Delete(path);

            // act
            //set base dir
            renamer.SetBaseDirectory(baseDir!);
            //set output dir
            renamer.SetOutputDirectory(path);
            //check if new folder exists
            var result = Directory.Exists(path);

            // assert
            Assert.False(result);
            // delete test folder
            if (Directory.Exists(path)) Directory.Delete(path);
        }
    }
}