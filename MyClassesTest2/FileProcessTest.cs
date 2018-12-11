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
        private const string OK_FILE_Name = @"C:\ws\coderepo\MyClasses\MyClassesTest2\TestFile.txt";
        private string _GoodFileName;


        #region Class Initialize and Cleanup
        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize]
        public static void ClassInitialize(TestContext tc)
        {
            tc.WriteLine("In the Class Initialize.");
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {

        }

        #endregion

        #region Test Initialize and Cleanup


        // Use TestInitialize to run code before running each test 

        [TestInitialize]
        public void TestInitialize()
        {
            // kontrollera och kör allt som börjar med namnet nedan"
            if (TestContext.TestName.StartsWith("FileNameDoesExist"))
            {
                SetGoodFileName();
                if (!string.IsNullOrEmpty(_GoodFileName))
                {
                    TestContext.WriteLine("Creating File: " + _GoodFileName);
                    File.AppendAllText(_GoodFileName, "Hej Adam Rashid, en fil är skapad för test syfte");
                }

            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (TestContext.TestName.StartsWith("FileNameDoesExist"))
            {
                if (!string.IsNullOrEmpty(_GoodFileName))
                {
                    TestContext.WriteLine("Deleting file : " + _GoodFileName);
                    File.Delete(_GoodFileName);
                }
            }
        }



        #endregion



        public TestContext TestContext { get; set; }


        public void SetGoodFileName()
        {
            _GoodFileName = ConfigurationManager.AppSettings["GoodFileName"];
            if (_GoodFileName.Contains("[AppPath]"))
            {
                _GoodFileName = _GoodFileName.Replace("[AppPath]", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            }
        }


        [TestMethod]
        [DataSource("System.Data.SqlClient", "Server=(localdb)\\MSSQLLocalDB; Database=Sandbox;Integrated Security=True", "tests.FileProcessTest", DataAccessMethod.Sequential)] // check the appsettings file. 
        //[DataSource("Sandbox")] // this is not working on VS 2017
        public void FileExistsTestFromDB()
        {
            FileProcess fp = new FileProcess();
            string fileName;
            bool expectedValue;
            bool causesException;
            bool fromCall;

            // Get values from data row
            fileName        = TestContext.DataRow["FileName"].ToString();
            expectedValue   = Convert.ToBoolean(TestContext.DataRow["ExpectedValue"]);
            causesException = Convert.ToBoolean(TestContext.DataRow["CausesException"]);

            // Check assertion
            try
            {
                fromCall = fp.FileExists(fileName);
                Assert.AreEqual(expectedValue, fromCall,
                  "File Name: " + fileName +
                  " has failed it's existence test in test: FileExistsTestFromDB()");
            }
            catch (AssertFailedException ex)
            {
                // Rethrow assertion
                throw ex;
            }
            catch (ArgumentNullException)
            {
                // See if method was expected to throw an exception
                Assert.IsTrue(causesException);
            }
        }


        //[Description("Test if File Exist")] // den klottrar mycket .. bättre att inte ha .
        [Owner("Adam Rashid")]
        [Priority(0)]
        //[TestCategory("")] //för gruppering syfte endast.
        [TestMethod]
        public void FileNameDoesExist()
        {
            //TODO: Arrange
            var fp = new FileProcess();


            // TODO: Acting 

            //SetGoodFileName();

            //TestContext.WriteLine("Creating the file: " + _GoodFileName);

            // skapar en fil med filnamnet, och skriver en viss text i filen.
            //File.AppendAllText(_GoodFileName, "This is a test med from unitest by Adam Rashid");



            // loggar  om filen existerar eller inte
            TestContext.WriteLine("Testing the file: " + _GoodFileName);
            var fromCall = fp.FileExists(_GoodFileName);

            // loggar att den kommer radera filen
            //TestContext.WriteLine("Deleting the file: " + _GoodFileName);

            // Raderar filen som skapades innan.
            //File.Delete(_GoodFileName);


            // TODO: Assert            
            Assert.IsTrue(fromCall, "Yes, The file {0} does Exists", _GoodFileName);
        }

        [TestMethod]
        [Priority(0)]
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
        [Priority(0)]
        // Detta är istället för Assert, då vi testar om metoden generar en viss typ av Excpetion.
        [ExpectedException(typeof(ArgumentNullException))]
        public void FileNameNullOrEmpty_ThrowArgumentNullException()
        {
            // Arrange
            var fp = new FileProcess();


            //Act/Acting k
            var fileName = "";
            var fromCall = fp.FileExists(fileName);

            // Assert
            // No assert since we expect argumentNullException

        }

        [TestMethod]
        [Priority(0)]
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
