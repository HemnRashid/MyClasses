using System;
using System.Configuration;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyClasses;

namespace MyClassesTest2
{
    [TestClass]
    public class FileProcessTest
    {
        private const string BAD_FILE_NAME = @"Adam.txt";
        private const string OK_FILE_Name  = @"C:\ws\coderepo\MyClasses\MyClassesTest2\TestFile.txt";
        private string _GoodFileName;

        public TestContext TestContext { get; set; }


        public void SetGoodFileName()
        {
            _GoodFileName = ConfigurationManager.AppSettings["GoodFileName"];
            if (_GoodFileName.Contains("[AppPath]"))
            {                
                _GoodFileName= _GoodFileName.Replace("[AppPath]", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            }
        }



        [TestMethod]
        public void FileNameDoesExist()
        {
            // Arrange
            var fp = new FileProcess();


            // Acting 
            
            SetGoodFileName();

            TestContext.WriteLine("Creating the file: " + _GoodFileName);

            File.AppendAllText(_GoodFileName, "This is a test med from unitest by Adam Rashid");

            TestContext.WriteLine("Testing the file: " + _GoodFileName);
            var fromCall = fp.FileExists(_GoodFileName);

            TestContext.WriteLine("Deleting the file: " + _GoodFileName);
            File.Delete(_GoodFileName);


            // Assert
            Assert.IsTrue(fromCall);
        }

        [TestMethod]
        public void FileNameDoesNotExist()
        {
            // Arrange
            var fp = new FileProcess();


            //Act/Acting 
            
            var fromCall = fp.FileExists(BAD_FILE_NAME);

            // Assert
            Assert.IsFalse(fromCall);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FileNameNullOrEmpty_ThrowArgumentNullException()
        {
            // Arrange
            var fp = new FileProcess();


            //Act/Acting 
            var fileName = "";
            var fromCall = fp.FileExists(fileName);

            // Assert
            // No assert since we expect argumentNullException

        }

        [TestMethod]

        public void FileNameNullOrEmpty_ThrowArgumentNullException_UsingTryCatch()
        {
            // Arrange
            var fp = new FileProcess();

            //Act/Acting 
            try
            {
                var fileName = string.Empty; //empty string
                var fromCall = fp.FileExists(fileName);

            }
            catch (ArgumentNullException)
            {

                // if som, the test was success
                return;

            }


            // Assert
            // tala om varför det blev fel om ArgumentnullException inte passerar testet.
            Assert.Fail("Call to fileExists did not throw ArgumentNullException");

        }
    }
}
