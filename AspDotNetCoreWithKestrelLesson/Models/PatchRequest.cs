namespace AspDotNetCoreWithKestrelLesson.Models;

public class PatchRequest<T> : List<PatchOperation<T>>
{
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