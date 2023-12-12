using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Animations;

/// <summary>
/// Listens for touch events and performs an AR raycast from the screen touch point.
/// AR raycasts will only hit detected trackables like feature points and planes.
///
/// If a raycast hits a trackable, the <see cref="placedPrefab"/> is instantiated
/// and moved to the hit position.
/// </summary>
[RequireComponent(typeof(ARRaycastManager))]
public class MeasureDistances : MonoBehaviour {
	[SerializeField]
	[Tooltip("Instantiates this prefab on a plane at the touch location.")]
	GameObject m_PlacedPrefab;

	/// <summary>
	/// The prefab to instantiate on touch.
	/// </summary>
	public GameObject placedPrefab {
		get { return m_PlacedPrefab; }
		set { m_PlacedPrefab = value; }
	}

	/// <summary>
	/// The object instantiated as a result of a successful raycast intersection with a plane.
	/// </summary>
	public GameObject spawnedObject { get; private set; }

	/// <summary>
	/// The first-person camera being used to render the passthrough camera image (i.e. AR
	/// background).
	/// </summary>
	public Camera FirstPersonCamera;

	/// <summary>
	/// A prefab to place when a raycast from a user touch hits a plane.
	/// </summary>
	public GameObject pointPrefab;

	/// <summary>
	/// A prefab to place to unite two adiacent points.
	/// </summary>
	public GameObject linePrefab;

	/// <summary>
	/// A prefab to place to display the distance between two adiacent points.
	/// </summary>
	public TMP_Text textPrefab;

	/// <summary>
	/// The canvas needed to display text on screen.
	/// </summary>
	public Canvas parent;

	/// <summary>
	/// A list of all added points on screen.
	/// </summary>
	public List<GameObject> points = new List<GameObject>();

	/// <summary>
	/// A list of distances of adiacent points on screen.
	/// </summary>
	public List<TMP_Text> distances = new List<TMP_Text>();

	/// <summary>
	/// The total distance of all points on screen.
	/// </summary>
	public TMP_Text totalDistance;
	float distanceSum = 0;

	void Awake() {
		m_RaycastManager = GetComponent<ARRaycastManager>();
	}

	bool TryGetTouchPosition(out Vector2 touchPosition) {
		Touch touch;
		if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began) {
			touchPosition = default;
			return false;
		}

		if (Input.touchCount > 0) {
			touchPosition = Input.GetTouch(0).position;
			return true;
		}

		touchPosition = default;
		return false;
	}

	void Update() {
		/* The distance between two points needs to be always placed near the two points.
         * If we move the phone screen the text will also move (the text is fixed on canvas
         * and the canvas always follows the phone screen),
         * so we need to update the position on screen for each distance displayed.
         */
		foreach (Tuple<GameObject, TMP_Text> instDist in m_Lines) {
			/* TODO 2.3 Update text position - SOLVE THIS AFTER TESTING 2.1 - 2.2 AND NOTICE THE DIFFERENCES */
			Vector3 pos = instDist.Item1.transform.position;
			pos.y += 1.0f / 20.0f;
			
			instDist.Item2.transform.position = Camera.main.WorldToScreenPoint(pos);
		}

		if (!TryGetTouchPosition(out Vector2 touchPosition))
			return;

		if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon)) {
			/* Raycast hits are sorted by distance, so the first one
             * will be the closest hit.
             */
			var hitPose = s_Hits[0].pose;
			spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);

			/* Add a point to the list of points */
			points.Add(spawnedObject);

			/* If there is more than one point on screen, we can compute the distance */
			if (points.Count > 1) {
				/* Draw a line between the last two added points */
				/* TODO 1.1 Instantiate linePrefab */
				GameObject line = linePrefab;
				GameObject instance = Instantiate(line, line.transform.position, Quaternion.identity);


				/* TODO 1.2 Set position at half the distance between the last two added points */
				instance.transform.position = (points[points.Count - 1].transform.position + points[points.Count - 2].transform.position) / 2;


				/* TODO 1.3 Set rotation: use LookAt function to make the line oriented between the two points */
				instance.transform.LookAt(points[points.Count - 2].transform.position);


				/* TODO 1.4 Set scale: two fixed numbers on ox and oy axis, the distance between the two points on oz axis */
				instance.transform.localScale = new Vector3(0.5f, 0.5f, 20.0f * Vector3.Distance(points[points.Count - 1].transform.position, points[points.Count - 2].transform.position)); /* !0.5 can be changed to whatever value we want
                                                                            * !In order to correctly set the oz axis, pay attention
                                                                            * to the structure of the used prefab
                                                                            */

				/* TODO 1.5 Add an anchor to the line - SOLVE THIS AFTER TESTING 1.1 - 1.4 AND NOTICE THE DIFFERENCES */
				instance.AddComponent<ARAnchor>();

				/* Show on each line the distance */
				/* Instantiate textPrefab */
				TMP_Text partialDistance = Instantiate(textPrefab);
				/* Set the canvas as parent so that the text is displayed on screen */
				partialDistance.transform.SetParent(parent.transform);
				/* Set the position of the text on screen */
				partialDistance.transform.position = Camera.main.WorldToScreenPoint(new Vector3(line.transform.position.x,
					line.transform.position.y, line.transform.position.z));
				/* TODO 2.1 Compute the distance between the last two added points */
				float distance = Vector3.Distance(points[points.Count - 1].transform.position, points[points.Count - 2].transform.position);
				/* TODO 2.2 Add the distance to our distances list */
				partialDistance.text = "" + distance;
				distances.Add(partialDistance);

				m_Lines.Add(new Tuple<GameObject, TMP_Text>(instance, partialDistance));

				/* TODO 3 Update the total distance */
				distanceSum += distance;
				totalDistance.text = "Total distance: " + distanceSum;
			}
		}
	}

	private List<Tuple<GameObject, TMP_Text>> m_Lines = new();
	static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

	ARRaycastManager m_RaycastManager;
}