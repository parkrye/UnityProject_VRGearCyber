using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KSI
{
    public class Magazine : MonoBehaviour
    {
        public int numberOfBullet;
		//public Image magazineImage;
		//public Text magazineText;

		public List<GameObject> bullets;

		private void Start()
		{
			bullets = new List<GameObject>(numberOfBullet);
		}
	}
}
