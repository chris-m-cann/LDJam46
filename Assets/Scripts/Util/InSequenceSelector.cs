namespace Util
{
    public class InSequenceSelector
    {
        private int _index = -1;

        public T Select<T>(T[] list)
        {
            _index = (_index + 1) % list.Length;
            return list[_index];
        }
    }
}