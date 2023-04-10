namespace CrudTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            MyMath my = new MyMath();
            int a=5 ; int b = 10;
            int expected = 15;
            int res = my.Add(a, b);
            Assert.Equal(expected, res);

        }
    }
}