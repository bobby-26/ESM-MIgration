using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class UserControlCountryList : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lstCountry.DataSource = PhoenixRegistersCountry.ListCountry(1); //Active Country
            lstCountry.DataBind();

        }
    }

    public string selectedlist
    {
        get
        {
            StringBuilder strlist = new StringBuilder();
            foreach (RadListBoxItem item in lstCountry.Items)
            {
                if (item.Checked == true)
                {

                    strlist.Append(item.Value.ToString());
                    strlist.Append(",");
                }

            }
            if (strlist.Length > 1)
            {
                strlist.Remove(strlist.Length - 1, 1);
            }
            return strlist.ToString();
        }
        set
        {
            //StringBuilder strlist = new StringBuilder();
            //foreach (RadListBoxItem item in lstCountry.Items)
            //{
            //    if (item.Checked == true)
            //    {

            //        strlist.Append(item.Value.ToString());
            //        strlist.Append(",");
            //    }

            //}
            //if (strlist.Length > 1)
            //{
            //    strlist.Remove(strlist.Length - 1, 1);
            //}
            //value = strlist.ToString();

            string strlist = "," + value + ",";
            foreach (RadListBoxItem item in lstCountry.Items)
            {
                item.Checked = strlist.Contains("," + item.Value + ",") ? true : false;
            }
        }
    }
    public DataSet CountryList
    {
        set
        {
            lstCountry.Items.Clear();
            lstCountry.DataSource = value;
            lstCountry.DataBind();
        }
    }
    public string AppendDataBoundItems
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                lstCountry.AppendDataBoundItems = true;
            else
                lstCountry.AppendDataBoundItems = false;
        }
    }


    public string CssClass
    {
        set
        {
            chkboxlist.Attributes["class"] = value;
            lstCountry.CssClass = value;
        }
    }
    public Unit Width
    {
        set
        {
            lstCountry.Width = Unit.Parse(value.ToString());
            chkboxlist.Attributes.Add("style", "overflow-y: auto; overflow-x: hidden;height: 80px; width:" + value);
        }
        get
        {
            return lstCountry.Width;
        }
    }


    public string SelectedCountryName
    {
        get
        {
            return lstCountry.SelectedItem.Text;
        }
    }

    public string SelectedCountryValue
    {
        get
        {

            return lstCountry.SelectedValue;
        }
        set
        {
            lstCountry.SelectedIndex = -1;
            if (value != null && value != "" && value != "0")
                lstCountry.SelectedValue = value;

        }
    }
    public int SelectedValue
    {
        get
        {

            return (int.Parse(lstCountry.SelectedValue));
        }
        set
        {
            value = (int.Parse(lstCountry.SelectedValue));
        }
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void lstCountry_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }
}
