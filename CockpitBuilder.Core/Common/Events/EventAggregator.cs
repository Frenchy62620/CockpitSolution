﻿using System.Linq;
using CockpitBuilder.Core.Common.Extensions;

namespace CockpitBuilder.Core.Common.Events
{
    public class EventAggregator : IEventAggregator
    {
        private readonly WeakReferenceList<object> subscribers = new WeakReferenceList<object>();

        public void Subscribe(object subsriber)
        {
            subscribers.Add(subsriber);
        }

        public void Publish<T>(T message) where T : class
        {
            subscribers
                .OfType<IHandle<T>>()
                .ForEach(s => s.Handle(message));
        }
    }
}
