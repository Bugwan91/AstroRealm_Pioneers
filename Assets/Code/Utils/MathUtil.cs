using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace Code.Utils
{
    public struct MathUtil
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 Rotate(float3 v, float4 q, float3 t)
        {
            return new float3(
                v.x * q.x * q.x
                +v.y * q.x * q.y * 2f
                +v.z * q.x * q.z * 2f
                +v.x * q.w * q.w
                +v.z * q.y * q.w * 2f
                -v.y * q.z * q.w * 2f
                -v.x * q.z * q.z
                -v.x * q.y * q.y
                +t.x,
                
                v.x * q.x * q.y * 2f
                +v.y * q.y * q.y
                +v.z * q.y * q.z * 2f
                +v.x * q.z * q.w * 2f
                -v.y * q.z * q.z
                +v.y * q.w * q.w
                -v.z * q.x * q.w * 2f
                -v.y * q.x * q.x
                +t.y,
                
                v.x * q.x * q.z * 2f
                +v.y * q.y * q.z * 2f
                +v.z * q.z * q.z
                -v.x * q.y * q.w * 2f
                -v.z * q.y * q.y
                +v.y * q.x * q.w * 2f
                -v.z * q.x * q.x
                +v.z * q.w * q.w
                +t.z
            );
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 RotateY(float3 v, float4 q, float3 t)
        {
            return new float3(
                +v.x * q.w * q.w
                +v.z * q.y * q.w * 2f
                -v.x * q.y * q.y
                +t.x,
                
                +v.y * q.y * q.y
                +v.y * q.w * q.w
                +t.y,
                
                -v.x * q.y * q.w * 2f
                -v.z * q.y * q.y
                +v.z * q.w * q.w
                +t.z
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static quaternion Rotate(quaternion q1, quaternion q2)
        {
            var a = q1.value;
            var b = q2.value;
            return new quaternion(
                a.w*b.x + a.x*b.w + a.y*b.z - a.z*b.y,
                a.w*b.y + a.y*b.w + a.z*b.x - a.x*b.z,
                a.w*b.z + a.z*b.w + a.x*b.y - a.y*b.x,
                a.w*b.w - a.x*b.x - a.y*b.y - a.z*b.z
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 RayTraceToMainPlane(float3 from, float3 direction)
        {
            var planePoint = float3.zero;
            var planeNormal = new float3(0, 1, 0);

            var diff = from - planePoint;
            var prod1 = math.dot(diff, planeNormal);
            var prod2 = math.dot(direction, planeNormal);
            // TODO: Question to future myself: is it right to return PositiveInfinity? And should back point be a valid result?
            if (prod2 == 0f) return new float3(0, 0, float.PositiveInfinity);
            var prod3 = prod1 / prod2;
            return from - direction * prod3;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Angle(float3 a, float3 b)
        {
            return math.acos((a.x*b.x + a.y*b.y + a.z*b.z) / (math.length(a) * math.length(b)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float AngleY(float3 a, float3 b)
        {
            return math.acos(
                (a.x * b.x + a.z * b.z)
                / (math.sqrt(a.x * a.x + a.z * a.z) * math.sqrt(b.x * b.x + b.z * b.z)))
                * math.sign(math.cross(a, b).y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 ClampVector(float3 vector, float value)
        {
            var l = math.length(vector);
            if (l < value) return vector;
            return (vector / l) * value;

        }
    }
}