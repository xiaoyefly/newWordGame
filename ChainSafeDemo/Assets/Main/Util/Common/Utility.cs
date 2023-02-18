

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


	public static class Utility
	{

		/// <summary>
		/// 获取Transform的世界坐标
		/// </summary>
		/// <param name="rectTransform"></param>
		/// <param name="canvas"></param>
		/// <returns></returns>
		public static Vector3 TransformPoint2World(Transform transform, Vector3 point)
		{
			return transform.localToWorldMatrix.MultiplyPoint(point);
		}

		/// <summary>
		/// 获取Transform的本地坐标
		/// </summary>
		/// <param name="transform"></param>
		/// <param name="point"></param>
		/// <returns></returns>
		public static Vector3 TransformWorld2Point(Transform transform, Vector3 point)
		{
			return transform.worldToLocalMatrix.MultiplyPoint(point);
		}

		public static T GetOrAddComponent<T>(this Transform transform)
			 where T : Component
		{
			T t = transform.GetComponent<T>();
			return t ?? transform.gameObject.AddComponent<T>();
		}

	}

