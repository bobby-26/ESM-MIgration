using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class UserControlHotelRoom : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private string _selectedValue = "";
    private int _selectedBeds = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }
    public DataSet RoomList
    {
        set
        {
            ddlHotelRoom.Items.Clear();
            ddlHotelRoom.DataSource = value;
            ddlHotelRoom.DataBind();
            foreach (RadComboBoxItem item in ddlHotelRoom.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }
    public int RoomBeds
    {
        set
        {
            _selectedBeds = value;
        }
    }
    public string CssClass
    {
        set
        {
            ddlHotelRoom.CssClass = value;
        }
        get { return ddlHotelRoom.CssClass; }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlHotelRoom.AutoPostBack = true;
        }
    }

    public string SelectedRoomTypeName
    {
        get
        {
            return ddlHotelRoom.SelectedItem.Text;
        }
    }
    public string SelectedRoomID
    {
        get
        {
            return ddlHotelRoom.SelectedValue;
        }
        set
        {
            if (value == string.Empty || value == "Dummy")
            {
                ddlHotelRoom.SelectedIndex = -1;
                ddlHotelRoom.ClearSelection();
                ddlHotelRoom.Text = string.Empty;
                return;
            }
            _selectedValue = value;
            ddlHotelRoom.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlHotelRoom.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }

        }
    }
    public string SelectedValue
    {
        get
        {
            return ddlHotelRoom.SelectedValue.ToString();
        }
        set
        {
            _selectedValue = value;
            ddlHotelRoom.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlHotelRoom.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }
    public void bind()
    {
        ddlHotelRoom.DataSource = PhoenixRegistersHotelRoom.HotelRoomList(null);
        ddlHotelRoom.DataBind();

        foreach (RadComboBoxItem item in ddlHotelRoom.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }

    protected void ddlHotelRoom_DataBound(object sender, EventArgs e)
    {
        ddlHotelRoom.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));     
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }
    protected void ddlHotelRoom_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }
}
