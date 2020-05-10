using System;

namespace Util
{
    public abstract class Either<L, R>
    {
        public abstract bool IsLeft { get; }
        public abstract bool IsRight { get; }
        public abstract void IfLeft(Action<L> action);
        public abstract void IfRight(Action<R> action);
        public abstract TResult Match<TResult>(Func<L, TResult> Left, Func<R, TResult> Right);
        public abstract void Match(Action<L> Left, Action<R> Right);


    }
    public class Left<L, R> : Either<L, R>
    {
        private readonly L value;

        public Left(L left)
        {
            this.value = left;
        }

        public override bool IsLeft => true;
        public override bool IsRight => false;

        public override void IfLeft(Action<L> action)
        {
            action(value);
        }

        public override void IfRight(Action<R> action) { }

        public override TResult Match<TResult>(Func<L, TResult> Left, Func<R, TResult> Right)
        {
            return Left(value);
        }

        public override void Match(Action<L> Left, Action<R> Right)
        {
            Left.Invoke(value);
        }
    }

    public class Right<L, R> : Either<L, R>
    {
        private readonly R value;

        public Right(R value)
        {
            this.value = value;
        }

        public override bool IsLeft => false;
        public override bool IsRight => true;

        public override void IfLeft(Action<L> action) { }

        public override void IfRight(Action<R> action)
        {
            action(value);
        }

        public override TResult Match<TResult>(Func<L, TResult> Left, Func<R, TResult> Right)
        {
            return Right(value);
        }

        public override void Match(Action<L> Left, Action<R> Right)
        {
            Right.Invoke(value);
        }
    }
}