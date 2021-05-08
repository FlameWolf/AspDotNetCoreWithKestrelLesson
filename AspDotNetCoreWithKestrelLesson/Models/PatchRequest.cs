using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace AspDotNetCoreWithKestrelLesson.Models
{
	public class PatchRequest<T> : ICollection<PatchOperation<T>>
	{
		private readonly List<PatchOperation<T>> _requests;

		public PatchRequest()
		{
			_requests = new();
		}

		public int Count => _requests.Count;

		public bool IsReadOnly => false;

		public void Add(PatchOperation<T> item) => _requests.Add(item);

		public void Clear() => _requests.Clear();

		public bool Contains(PatchOperation<T> item) => _requests.Contains(item);

		public void CopyTo(PatchOperation<T>[] array, int arrayIndex) => _requests.CopyTo(array, arrayIndex);

		public bool Remove(PatchOperation<T> item) => _requests.Remove(item);

		public IEnumerator<PatchOperation<T>> GetEnumerator() => _requests.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => _requests.GetEnumerator();

		public static implicit operator JsonPatchDocument(PatchRequest<T> requests)
		{
			var patchDocument = new JsonPatchDocument();
			foreach (var request in requests)
			{
				patchDocument.Operations.Add
				(
					new Operation
					{
						op = request.Op,
						from = request.From,
						path = request.Path,
						value = request.Value
					}
				);
			}
			return patchDocument;
		}
	}
}