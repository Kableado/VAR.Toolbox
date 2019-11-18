﻿using System;
using System.Collections.Generic;

namespace VAR.Toolbox.Code
{
    public static class EventDispatcher
    {
        private static List<IEventListener> _eventListeners = null;

        private static IEnumerable<IEventListener> GetEventListeners()
        {
            if (_eventListeners != null)
            {
                return _eventListeners;
            }

            Type iEventListener = typeof(IEventListener);
            IEnumerable<Type> eventListeners = ReflectionUtils.GetTypesOfInterface(iEventListener);
            _eventListeners = new List<IEventListener>();
            foreach (Type eventListener in eventListeners)
            {
                IEventListener eventListenerInstance = Activator.CreateInstance(eventListener) as IEventListener;
                if (eventListenerInstance != null)
                {
                    _eventListeners.Add(eventListenerInstance);
                }
            }

            return _eventListeners;
        }

        public static void EmitEvent(string eventName, object eventData)
        {
            IEnumerable<IEventListener> eventListeners = GetEventListeners();
            foreach (IEventListener eventListener in eventListeners)
            {
                eventListener.ProcessEvent(eventName, eventData);
            }
        }

    }
}
