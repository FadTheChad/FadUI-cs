using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace FadUI.Core;

public enum Easing
{
    Linear,
    EaseIn,
    EaseOut,
    EaseInOut
}

public class AnimationManager
{
    private static List<ITween> _activeTweens = new();

    public static void Update(float dt)
    {
        for (int i = _activeTweens.Count - 1; i >= 0; i--)
        {
            if (_activeTweens[i].Update(dt))
            {
                _activeTweens.RemoveAt(i);
            }
        }
    }

    public static void TweenColor(Func<Color> getter, Action<Color> setter, Color target, float duration, Easing easing = Easing.EaseInOut)
    {
        var existing = _activeTweens.Find(t => t is ColorTween ct && ct.Matches(setter)) as ColorTween;
        if (existing != null)
        {
            if (existing.IsHeadingTo(target)) return;
            _activeTweens.Remove(existing);
        }
        _activeTweens.Add(new ColorTween(getter(), target, duration, setter, easing));
    }

    public static void TweenFloat(Func<float> getter, Action<float> setter, float target, float duration, Easing easing = Easing.EaseInOut)
    {
        var existing = _activeTweens.Find(t => t is FloatTween ft && ft.Matches(setter)) as FloatTween;
        if (existing != null)
        {
            if (MathF.Abs(existing.TargetValue - target) < 0.001f) return;
            _activeTweens.Remove(existing);
        }
        _activeTweens.Add(new FloatTween(getter(), target, duration, setter, easing));
    }

    public static void TweenVector2(Func<Vector2> getter, Action<Vector2> setter, Vector2 target, float duration, Easing easing = Easing.EaseInOut)
    {
        var existing = _activeTweens.Find(t => t is Vector2Tween vt && vt.Matches(setter)) as Vector2Tween;
        if (existing != null)
        {
            if (Vector2.Distance(existing.TargetValue, target) < 0.001f) return;
            _activeTweens.Remove(existing);
        }
        _activeTweens.Add(new Vector2Tween(getter(), target, duration, setter, easing));
    }

    private static float ApplyEasing(float t, Easing easing)
    {
        return easing switch
        {
            Easing.EaseIn => t * t * t,
            Easing.EaseOut => 1 - MathF.Pow(1 - t, 3),
            Easing.EaseInOut => t < 0.5f ? 4 * t * t * t : 1 - MathF.Pow(-2 * t + 2, 3) / 2,
            _ => t
        };
    }

    private interface ITween
    {
        bool Update(float dt);
    }

    private class FloatTween : ITween
    {
        private float _start, _target, _time, _duration;
        private Action<float> _setter;
        private Easing _easing;

        public float TargetValue => _target;

        public FloatTween(float s, float t, float d, Action<float> set, Easing easing)
        { _start = s; _target = t; _duration = d; _setter = set; _easing = easing; }

        public bool Matches(Action<float> other) => _setter == other;

        public bool Update(float dt)
        {
            _time += dt;
            float t = Math.Clamp(_time / _duration, 0, 1);
            _setter(_start + (_target - _start) * ApplyEasing(t, _easing));
            return t >= 1.0f;
        }
    }

    private class ColorTween : ITween
    {
        private Color _start, _target;
        private float _time, _duration;
        private Action<Color> _setter;
        private Easing _easing;

        public ColorTween(Color s, Color t, float d, Action<Color> set, Easing easing)
        { _start = s; _target = t; _duration = d; _setter = set; _easing = easing; }

        public bool Matches(Action<Color> other) => _setter == other;
        public bool IsHeadingTo(Color goal) =>
            _target.R == goal.R && _target.G == goal.G && _target.B == goal.B && _target.A == goal.A;

        public bool Update(float dt)
        {
            _time += dt;
            float t = Math.Clamp(_time / _duration, 0, 1);
            float easedT = ApplyEasing(t, _easing);
            _setter(new Color(
                (byte)Raymath.Lerp(_start.R, _target.R, easedT),
                (byte)Raymath.Lerp(_start.G, _target.G, easedT),
                (byte)Raymath.Lerp(_start.B, _target.B, easedT),
                (byte)Raymath.Lerp(_start.A, _target.A, easedT)
            ));
            return t >= 1.0f;
        }
    }

    private class Vector2Tween : ITween
    {
        private Vector2 _start, _target;
        private float _time, _duration;
        private Action<Vector2> _setter;
        private Easing _easing;

        public Vector2 TargetValue => _target;

        public Vector2Tween(Vector2 s, Vector2 t, float d, Action<Vector2> set, Easing easing)
        { _start = s; _target = t; _duration = d; _setter = set; _easing = easing; }

        public bool Matches(Action<Vector2> other) => _setter == other;

        public bool Update(float dt)
        {
            _time += dt;
            float t = Math.Clamp(_time / _duration, 0, 1);
            _setter(Vector2.Lerp(_start, _target, ApplyEasing(t, _easing)));
            return t >= 1.0f;
        }
    }
}