using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WiimoteApi;

public class PlayerController : MonoBehaviour {

	public float speed;
	public float torque;
	public float[] zeros;
	public Vector3 origin;
	private Rigidbody rb;
	public Wiimote r;
	private int i=0;
	public bool active=false;
	public bool setup = false;
	public bool dataSet = false;
	bool upCal=false;
	bool expCal=false;
	bool leftCal=false;
	public float[][] cal;
		
	void Start ()
	{   

		rb = GetComponent<Rigidbody>();
		origin = rb.position;

	}
	void setupR(){
		int data;
		if (r == null) {
			WiimoteManager.FindWiimotes ();
			if (WiimoteManager.Wiimotes [0] != null) {
				r = WiimoteManager.Wiimotes [0];
			}
		}
		if (WiimoteManager.HasWiimote ()&& !setup) {
			r.SendPlayerLED (true, false, false, false);
			data = r.ReadWiimoteData ();
			if (data != 0) {
				setup = true;
				print ("Wiimote Data: "+data+"\n"+setup);
			}
		}
	}

	int readData(){
		int data;
		data = r.ReadWiimoteData ();
		if (data != 0) {
			//print ("Wiimote Data: "+data);
		}
		return data;
	}

	void setSendType(){
		int data;
		if (setup&&!dataSet) {
			data=r.SendDataReportMode (InputDataType.REPORT_BUTTONS_ACCEL);
			if (data >=1) {
				dataSet = true;
				print("DataReportType Set: "+data+"\n"+setup+"\n"+dataSet);

			}
		}
	}

	int checkButtons(){
		int x=0;
		bool a = r.Button.a;
		bool b = r.Button.b;
		bool up = r.Button.d_up;
		bool down = r.Button.d_down;
		bool left = r.Button.d_left;
		bool right = r.Button.d_right;

		if (a) {
			print("Button A");
			x = 1;
		}
		if (b) {
			print ("Button B");
			x = 2;

		}
		if (up) {
			print ("Button Up");
			x = 3;
		}
		if (down) {
			print ("Button Down");
			x = 4;
		}
		if (left) {
			print ("Button Left");
			x = 5;
		}
		if (right) {
			print ("Button Right");
			x = 6;
		}
		return x;
	}

	void calibrate(){
		int data;
		data=readData ();
		if (data != 0) {
			if (checkButtons () == 1) {
				r.Accel.CalibrateAccel (AccelCalibrationStep.A_BUTTON_UP);
				upCal = true;
			}
			if (checkButtons () == 2) {
				r.Accel.CalibrateAccel (AccelCalibrationStep.EXPANSION_UP);
				expCal = true;
			}
			if (checkButtons () == 3) {
				r.Accel.CalibrateAccel (AccelCalibrationStep.LEFT_SIDE_UP);
				leftCal = true;
			}
		}
	
	}

	void checkCal(){
		StringBuilder str = new StringBuilder();
		if (upCal && expCal && leftCal) {
			for (int x = 0; x < 3; x++)
			{
				for (int y = 0; y < 3; y++)
				{
					cal[y][x]=r.Accel.accel_calib[y, x];
					str.Append(r.Accel.accel_calib[y, x]).Append(" ");
				}
				str.Append("\n");
			}
			print (str);
		}
	}

	Vector3 getAccel(){
		Vector3 ret;
		float[] accel;
		float a_x;
		float a_y;
		float a_z;
		accel = r.Accel.GetCalibratedAccelData ();
		a_x = accel [0];
		a_y = accel [2];
		a_z = accel [1];
		ret = new Vector3(a_x, a_z, a_y);
		return ret;
		}

	void Update ()
	{	
		Vector3 force;
		
		i += 1;
		if (i % 10 == 0) {
			//print ("Status: "+setup+" "+dataSet);
			setupR ();
			if (setup) {
				setSendType ();
				calibrate ();
				force = getAccel ();
				rb.AddTorque (force * torque);
				rb.AddForce (force * speed);
			}

		}
	}
	void OnApplicationQuit(){
		//WiimoteManager.Cleanup (r);
	
	}
		
}