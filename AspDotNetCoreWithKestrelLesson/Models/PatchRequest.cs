using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace AspDotNetCoreWithKestrelLesson.Models
{
	public class PatchRequest<T> : IList<PatchOperation<T>>
	{
		private readonly List<PatchOperation<T>> _requests;

		public PatchRequest()
		{
			_requests = new();
		}

		PatchOperation<T> IList<PatchOperation<T>>.this[int index]
		{
			set => _requests[index] = value;
			get => _requests[index];
		}

		public int Count => _requests.Count;

		public bool IsReadOnly => false;

		public void Add(PatchOperation<T> item) => _requests.Add(item);

		public void Clear() => _requests.Clear();

		public bool Contains(PatchOperation<T> item) => _requests.Contains(item);

		public void CopyTo(PatchOperation<T>[] array, int arrayIndex) => _requests.CopyTo(array, arrayIndex);

		public int IndexOf(PatchOperation<T> item) => _requests.IndexOf(item);

		public void Insert(int index, PatchOperation<T> item) => _requests.Insert(index, item);

		public bool Remove(PatchOperation<T> item) => _requests.Remove(item);

		public void RemoveAt(int index) => _requests.RemoveAt(index);

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