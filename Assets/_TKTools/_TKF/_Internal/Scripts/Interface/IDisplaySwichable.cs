using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TKF
{
    interface IDisplaySwichable
    {
        void Show();
        void Hide();
    }

    interface IDisplaySwichable<T>
    {
        void Show(T data);
        void Hide(T data);
    }

    interface IDisplaySwichable<T1, T2>
    {
        void Show(T1 data1, T2 data2);
        void Hide(T1 data1, T2 data2);
    }
}