namespace Day04
{
    /// <summary>
    /// Interface enabling the parser to tell us what it finds.
    /// </summary>
    /// <remarks>
    /// <para>
    /// I've reached that moment in the challenge where I really wish I was using F#. In
    /// particular, I'm missing sum types. However, if you don't mind operating in 'push'
    /// mode, interfaces are a tolerable way to represent the same structure of information.
    /// It sort of combines two aspects of F# into one: sum types (types that can be any of a
    /// fixed set of different things), and pattern matching on sum types. Implementations of this
    /// interface are in effect pattern matching on the notification.
    /// </para>
    /// </remarks>
    internal interface INotifications
    {
        void BeginShift(int guardId);
        void FallsAsleep();
        void WakesUp();
    }
}
