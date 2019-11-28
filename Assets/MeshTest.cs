using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTest : MonoBehaviour
{
    /// <summary>
    /// Checks if the specified ray hits the triagnlge descibed by p1, p2 and p3.
    /// Möller–Trumbore ray-triangle intersection algorithm implementation.
    /// </summary>
    /// <param name="p1">Vertex 1 of the triangle.</param>
    /// <param name="p2">Vertex 2 of the triangle.</param>
    /// <param name="p3">Vertex 3 of the triangle.</param>
    /// <param name="ray">The ray to test hit for.</param>
    /// <returns><c>true</c> when the ray hits the triangle, otherwise <c>false</c></returns>
    public static bool Intersect(Vector3 p1, Vector3 p2, Vector3 p3, Ray ray)
    {
        // Vectors from p1 to p2/p3 (edges)
        Vector3 e1, e2;  

        Vector3 p, q, t;
        float det, invDet, u, v;
  
        //Find vectors for two edges sharing vertex/point p1
        e1 = p2 - p1;
        e2 = p3 - p1;

        // calculating determinant 
        p = Vector3.Cross(ray.direction, e2);

        //Calculate determinat
        det = Vector3.Dot(e1, p);

        //if determinant is near zero, ray lies in plane of triangle otherwise not
        if (det > -Mathf.Epsilon && det < Mathf.Epsilon) { return false; }
        invDet = 1.0f / det;

        //calculate distance from p1 to ray origin
        t = ray.origin - p1;

        //Calculate u parameter
        u = Vector3.Dot(t, p) * invDet;

        //Check for ray hit
        if (u < 0 || u > 1) { return false; }

        //Prepare to test v parameter
        q = Vector3.Cross(t, e1);

        //Calculate v parameter
        v = Vector3.Dot(ray.direction, q) * invDet;

        //Check for ray hit
        if (v < 0 || u + v > 1) { return false; }

        if ((Vector3.Dot(e2, q) * invDet) > Mathf.Epsilon)
        { 
            //ray does intersect
            return true;
        }
 
        // No hit at all
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        Mesh initialMesh = this.GetComponent<MeshFilter>().mesh;

	DestructionMesh(initialMesh);
    }

    private static int PointInTriangle(Vector3 point, Vector3[] face)
    {
	for (int i = 0 ; i < face.Length; i++)
	{
		if (point.x == face[i].x && point.y == face[i].y && point.z == face[i].z)
		{
			return 1;
		}
	}
	return 0;
    }

    private static List<bool> GetDestroyedTriangles(Mesh obj, int i, List<bool> is_destroyed, int coef=256)
    {
	Vector3[] triangle = {
	     	  obj.vertices[obj.triangles[i]],
	     	  obj.vertices[obj.triangles[i + 1]],
		  obj.vertices[obj.triangles[i + 2]]
	};

	for (int idx = 0; idx < obj.triangles.Length; idx +=3)
	{
	     Vector3[] face = {
	     	       obj.vertices[obj.triangles[idx]],
	     	       obj.vertices[obj.triangles[idx + 1]],
	     	       obj.vertices[obj.triangles[idx + 2]]
	     };

	     int nb_similar = PointInTriangle(triangle[0], face) +
	     		      PointInTriangle(triangle[1], face) +
			      PointInTriangle(triangle[2], face);
	     if (0 < nb_similar && nb_similar < 3 && !is_destroyed[idx] && coef > 1)
	     {
		is_destroyed[idx] = true;
		is_destroyed = GetDestroyedTriangles(obj, idx, is_destroyed, coef / 2);
	     }
	}

	return is_destroyed;
    }

    private static List<Mesh> DestructionTriangle(Mesh obj, int i)
    {
	List<bool> is_destroyed = new List<bool>(obj.triangles.Length);
	for (int idx = 0; idx < 100; idx += 1)
	{
		is_destroyed.Add(false);
	}
	is_destroyed = GetDestroyedTriangles(obj, i, is_destroyed);

	List<Mesh> meshes = new List<Mesh>();

	Mesh new_obj = new Mesh();
	new_obj.uv = obj.uv;
	new_obj.uv2 = obj.uv2;
	new_obj.colors = obj.colors;
	List<Vector3> vertices = new List<Vector3>(obj.vertices);
	List<int> triangles = new List<int>();
	// new_obj.vertices = obj.vertices;
	// new_obj.triangles = obj.triangles;

	for (int idx = 0; idx < obj.triangles.Length; idx += 3)
	{
	     Vector3[] face = {
	     	       obj.vertices[obj.triangles[idx]],
	     	       obj.vertices[obj.triangles[idx + 1]],
	     	       obj.vertices[obj.triangles[idx + 2]]
	     };

	     if (is_destroyed[idx])
	     {

		// We create and add the new point
		Vector3 new_point = new Vector3((face[0].x + face[1].x + face[2].x) / 3,
						(face[0].y + face[1].y + face[2].y) / 3,
						(face[0].z + face[1].z + face[2].z) / 3
		);
		vertices.Add(new_point);

		// We create a new mesh
		// TODO: This may need to be optimized in further version
		Mesh new_mesh = new Mesh();
		new_mesh.uv = obj.uv;
		new_mesh.uv2 = obj.uv2;
		new_mesh.colors = obj.colors;
		new_mesh.SetVertices(vertices);
		List<int> new_mesh_triangles = new List<int>();

		// We add the 3 new triangles to the new mesh
		new_mesh_triangles.AddRange(new int[]{obj.triangles[idx],
						      obj.triangles[idx + 1],
					     	      vertices.Count - 1,
					     	      vertices.Count - 1,
					     	      obj.triangles[idx + 1],
					     	      obj.triangles[idx + 2],
					     	      obj.triangles[idx],
					     	      vertices.Count - 1,
					     	      obj.triangles[idx + 2]});

		new_mesh.triangles = new_mesh_triangles.ToArray();
		meshes.Add(new_mesh);
	     }
	     else
	     {
		// We only add the old triangle if it wasn't destroyed
		triangles.AddRange(new int[]{obj.triangles[idx],
					     obj.triangles[idx + 1],
					     obj.triangles[idx + 2]});
	     }
	}
	new_obj.SetVertices(vertices);
	new_obj.triangles = triangles.ToArray();


	meshes.Add(new_obj);
	return meshes;
    }

    /// <summary>
    /// Destroy an object given an impact.
    /// </summary>
    /// <param name="obj">Object to destroy.</param>
    /// <param name="anchorPoint">Impact point.</param>
    /// <param name="normalDirection">Direction of the impact.</param>
    /// <returns>A list of new mesh object</returns>
    private static List<Mesh> DestructionMesh(Mesh obj, Vector3 anchorPoint, Vector3 normalDirection)
    {
	 for (int i = 0; i < obj.triangles.Length; i += 3) {
	     bool result = Intersect(obj.vertices[obj.triangles[i]],
	     	  	   		  obj.vertices[obj.triangles[i + 1]],
					  obj.vertices[obj.triangles[i + 2]],
			    		  new Ray(anchorPoint, normalDirection));
	     if (result) {
	     	return DestructionTriangle(obj, i);
	     }
	 }
	 List<Mesh> ret = new List<Mesh>();
	 ret.Add(obj);
	 return ret;
    }

    /// This one is for debug
    /// The impact point is random
    private static List<Mesh> DestructionMesh(Mesh obj)
    {
	 return DestructionTriangle(obj, UnityEngine.Random.Range(0, obj.triangles.Length));
    }
}
