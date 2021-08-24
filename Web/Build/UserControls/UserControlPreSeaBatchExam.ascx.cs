using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class UserControlPreSeaBatchExam : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private string _selectedValue = "";
    private string _width = "";

    string batchid;
    string semesterid;
    string examid;
    string sectionid;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }    
    }
    public DataSet BatchExamList
    {
        set
        {
            ddlBatchExam.Items.Clear();
            ddlBatchExam.DataSource = value;
            ddlBatchExam.DataBind();
        }
    }
    public bool Enabled
    {
        set
        {
            ddlBatchExam.Enabled = value;
        }
    }
    public string CssClass
    {
        set
        {
            ddlBatchExam.CssClass = value;
        }
    }
    public string AppendDataBoundItems
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                _appenddatabounditems = true;
            else
                _appenddatabounditems = false;
        }
    }
    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlBatchExam.AutoPostBack = true;
        }
    }
    public string Clear
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlBatchExam.Items.Clear();
        }
    }
    public string BatchId
    {
        get
        {
            return batchid;
        }
        set
        {
            batchid = value;
        }
    }
    public string SectionId
    {
        get
        {
            return sectionid;
        }
        set
        {
            sectionid = value;
        }
    }
    public string SemesterId
    {
        get
        {
            return semesterid;
        }
        set
        {
            semesterid = value;
        }
    }
    public string ExamId
    {
        get
        {
            return examid;
        }
        set
        {
            examid = value;
        }
    }

    public void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }
    public void ddlBatchExam_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }
    public string SelectedBatchExam
    {
        get
        {
            return ddlBatchExam.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlBatchExam.SelectedIndex = -1;
                ddlBatchExam.ClearSelection();
                ddlBatchExam.Text = string.Empty;
                return;
            }
            _selectedValue = value;
            ddlBatchExam.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlBatchExam.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }    
    private void bind()
    {
        ddlBatchExam.DataSource = PhoenixPreSeaBatchExamSchedule.ListPreSeaBatchExamSchedule(General.GetNullableInteger(batchid)
            , General.GetNullableInteger(semesterid)
            , General.GetNullableInteger(examid)
            , General.GetNullableInteger(sectionid)
            , null
            );            
        ddlBatchExam.DataBind();

        foreach (RadComboBoxItem item in ddlBatchExam.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ddlBatchExam_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlBatchExam.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    public string SelectedName
    {
        get
        {

            return ddlBatchExam.SelectedItem.Text;
        }
        set
        {
            ddlBatchExam.SelectedIndex = -1;
            if (value != null && value != "" && value != "0")
                ddlBatchExam.SelectedItem.Text = value;
        }
    }

    public string Width
    {
        set
        {
            if (!string.IsNullOrEmpty(value))
                ddlBatchExam.Width = Unit.Parse(value);
        }
        get
        {
            return _width;
        }
    }
}
