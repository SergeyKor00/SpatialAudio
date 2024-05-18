using System;
using Core.Engine.Interfaces;
using UnityEngine;

namespace UnityView
{
    public class UnityTransformAudioSource : MonoBehaviour,  IAudioSource
    {
        private Transform _transform;
        public Vector2 Position => new(_transform.position.x, transform.position.z);

        private void Awake()
        {
            _transform = transform;
        }
    }
}