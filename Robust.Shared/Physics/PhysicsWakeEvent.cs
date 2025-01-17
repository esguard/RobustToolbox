using Robust.Shared.GameObjects;

namespace Robust.Shared.Physics
{
    [ByRefEvent]
    public readonly struct PhysicsWakeEvent
    {
        public readonly PhysicsComponent Body;

        public PhysicsWakeEvent(PhysicsComponent component)
        {
            Body = component;
        }
    }

    [ByRefEvent]
    public readonly struct PhysicsSleepEvent
    {
        public readonly PhysicsComponent Body;

        public PhysicsSleepEvent(PhysicsComponent component)
        {
            Body = component;
        }
    }
}
