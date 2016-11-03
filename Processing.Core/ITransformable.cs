namespace Processing.Core
{
    public interface ITransformable
    {
        void PushMatrix();
        void PopMatrix();

        void Translate(float x, float y);
        void Rotate(float r);
        void Scale(float s);
    }
}
