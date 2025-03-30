using System.Collections.Generic;
using UnityEngine;

namespace Gem.Systems
{
	public sealed class ResourcesSystem : MonoBehaviour
	{
		[SerializeField] private List<GameObject> _squaresPrefabs;

		public IEnumerable<GameObject> SquaresPrefabs => _squaresPrefabs;
	}
}
