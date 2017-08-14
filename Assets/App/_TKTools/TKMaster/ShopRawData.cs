using UnityEngine;
using TKF;
using TKMaster;
using System.Collections.Generic;
namespace Culsu{
/// <summary>
/// ShopRawDataのマスターデータのクラス
/// </summary>
[System.Serializable]
public class ShopRawData : RawDataBase
{
[SerializeField]
private string name;
public string Name{get{return name;}}
[SerializeField]
private int order;
public int Order{get{return order;}}
[SerializeField]
private string purchaseConsiderationType;
public string PurchaseConsiderationType{get{return purchaseConsiderationType;}}
[SerializeField]
private string purchaseType;
public string PurchaseType{get{return purchaseType;}}
[SerializeField]
private string productId;
public string ProductId{get{return productId;}}
[SerializeField]
private string footerTitle;
public string FooterTitle{get{return footerTitle;}}
[SerializeField]
private string puchaseValue;
public string PuchaseValue{get{return puchaseValue;}}
[SerializeField]
private string price;
public string Price{get{return price;}}
[SerializeField]
private string bestNotationText;
public string BestNotationText{get{return bestNotationText;}}
}
}
