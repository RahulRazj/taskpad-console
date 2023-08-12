using Utilities;

namespace console_taskpad_test
{
    public class Tests
    {
        string[] options;
        string evenLengthPadded;
        string oddLengthPadded;
        int padLength;

        [SetUp]
        public void Setup()
        {
            options = new string[] { "Get All Tasks", "View a Task", "Add Task", "Update Task", "Delete Task", "Load Tasks From File", "Save Tasks To File", "Get Specific Tasks", "Clear Console", "Exit" };

            evenLengthPadded = "  Id  ";
            oddLengthPadded = " 123  ";
            padLength = 6;
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        [Test]
        public void MathOptionsInTheUtilsOption()
        {
            string[] appOptions = Utils.GetMenuOptions();

            Assert.That(appOptions, Is.EquivalentTo(options));
        }

        [Test]
        public void GivenAStringOfEvenLengthPadTheStringEvenlyWithSpaces([Values("Id")] string input)
        {

            string output = My_Table.MyTable.PadStringWithSpaces(input, padLength);
            Assert.That(output, Is.EqualTo(evenLengthPadded));
        }

        [Test]
        public void GivenAStringOfOddLengthPadTheStringWithOneSpaceMoreOnTheEndOfString([Values("123")] string input)
        {
            string output = My_Table.MyTable.PadStringWithSpaces(input, padLength);
            Assert.That(output, Is.EqualTo(oddLengthPadded));
        }
    }
}