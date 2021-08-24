using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using System.Web.UI;
using System.Text;
using Telerik.Web.UI;

public partial class RegisterCrewPromotionConfigurationEdit : PhoenixBasePage
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Save", "SAVE", ToolBarDirection.Right);
            //toolbarsub.AddButton("List", "LIST", ToolBarDirection.Right);

            CrewPromotionMenu.AccessRights = this.ViewState;
            CrewPromotionMenu.MenuList = toolbarsub.Show();
            if (!IsPostBack)
            {
                ViewState["ID"] = "";
                ViewState["FLDLICENCELIST"] ="";
                ViewState["FLDTASKLIST"] = "";
                ViewState["FLDRANKFROM"] = "";

                if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() != "")
                {
                    ViewState["ID"] = Request.QueryString["id"].ToString();
                }
                if (!String.IsNullOrEmpty(ViewState["ID"].ToString()))
                    BindFields();
                else
                {
                    BindRankCheckbox(lstExpinRank, 2, null);
                    BindCOC(chkListLicence, null, null,null);
                    BindTasks(chkListTasks, null, null, null);
                    //BindVesselType(lstExpinVesseltype, 2, null);
                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    


    protected void CrewPromotionMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../Registers/RegisterCrewPromotionConfiguration.aspx");
            }
            else if (CommandName.ToUpper().Equals("SAVE"))
            {
                SaveMatrix();                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void ChkRankProm()
    {
        ucRankTo.Items.Clear();
        ucRankTo.SelectedValue = "";
        ucRankTo.Text = "";

        ucRankTo.DataSource = PhoenixRegistersRank.ListRankFilter(null, null, General.GetNullableInteger(ViewState["FLDRANKFROM"].ToString()), 0);
        ucRankTo.DataTextField = "FLDRANKNAME";
        ucRankTo.DataValueField = "FLDRANKID";
        ucRankTo.DataBind();
        ucRankTo.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }


    protected void BindFields()
    {
        DataTable dt = PhoenixRegisterCrewPromotionConfiguration.CrewPromotionConfigurationEdit(new Guid(ViewState["ID"].ToString()));
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            ucRankFrom.SelectedRank = "";            
            ucRankFrom.SelectedRank = dr["FLDRANKFROM"].ToString();        
            ViewState["FLDRANKFROM"] = dr["FLDRANKFROM"].ToString();
            ChkRankProm();
            ucRankTo.SelectedValue = dr["FLDRANKTO"].ToString();

            BindRankCheckbox(lstExpinRank, 2, dr["FLDRANKEXPERIENCELIST"].ToString());
            BindRankCheckbox(lstSelectedExpinRank, 1, dr["FLDRANKEXPERIENCELIST"].ToString());
 
            txtRankExp.Text = dr["FLDRANKEXPERIENCE"].ToString();
            txtAppraisal.Text = dr["FLDAPPRAISALRECOMMENDED"].ToString();
            //BindVesselType(lstExpinVesseltype, 2, dr["FLDEXPERIENCEVESSELTYPELIST"].ToString());
            //BindVesselType(lstSelectedExpinVesseltype, 1, dr["FLDEXPERIENCEVESSELTYPELIST"].ToString());
            //txtVesseltypeExp.Text = dr["FLDVESSELTYPEEXPERIENCE"].ToString();

            BindCOC(chkListLicence, null, null, dr["FLDRANKTO"].ToString());
            SetSelectedItemsForCheckBoxList(chkListLicence, dr["FLDLICENCELIST"].ToString());

            ViewState["FLDLICENCELIST"] = dr["FLDLICENCELIST"].ToString();
            ViewState["FLDTASKLIST"] = dr["FLDTASKLIST"].ToString();

            BindTasks(chkListTasks, null, null, dr["FLDRANKFROM"].ToString());
            SetSelectedItemsForCheckBoxList(chkListTasks, dr["FLDTASKLIST"].ToString());

        }
    }
    
    protected void BindRankCheckbox(RadListBox lst, int? includeexclude, string list)
    {
        lst.DataSource = PhoenixRegisterCrewPromotionConfiguration.CrewPromotionRankList(includeexclude, General.GetNullableString(list));
        lst.DataTextField = "FLDRANKNAME";
        lst.DataValueField = "FLDRANKID";
        lst.DataBind();
    }

    protected void BindCOC(RadListBox lst, int? includeexclude, string list, string appliedto)
    {

        lst.DataSource = PhoenixRegisterCrewPromotionConfiguration.CrewPromotionDocumentLicenceList(null, General.GetNullableInteger(appliedto), 1, null, null,
                                                                includeexclude, General.GetNullableString(list));
        lst.DataTextField = "FLDDOCUMENTNAME";
        lst.DataValueField = "FLDDOCUMENTID";
        lst.DataBind();

    }

    protected void BindTasks(RadListBox lst, int? includeexclude, string list, string appliedto)
    {

        lst.DataSource = PhoenixRegisterCrewPromotionConfiguration.CrewPromotionTaskist(General.GetNullableInteger(appliedto),
                                                                includeexclude, General.GetNullableString(list));
        lst.DataTextField = "FLDTASKNAME";
        lst.DataValueField = "FLDTASKID";
        lst.DataBind();
    }

    protected void ucRankTo_TextChangedEvent(object sender, EventArgs e)
    {

        BindCOC(chkListLicence, null, null,ucRankTo.SelectedValue);
        
        SetSelectedItemsForCheckBoxList(chkListLicence, ViewState["FLDLICENCELIST"].ToString());

    }

    protected void ucRankFrom_TextChangedEvent(object sender, EventArgs e)
    {
        ViewState["FLDRANKFROM"] = ucRankFrom.SelectedRank;
        
        ChkRankProm();

        BindTasks(chkListTasks, null, null, ucRankFrom.SelectedRank);
    }

    protected void SaveMatrix()
    {
        if (!IsValidPromotionConfig())
        {
            ucError.Visible = true;
            return;
        }

        ViewState["FLDLICENCELIST"] = GetSelectedItemsForCheckBoxList(chkListLicence);
        ViewState["FLDTASKLIST"] = GetSelectedItemsForCheckBoxList(chkListTasks);
        
        if (ViewState["ID"].ToString() == null || ViewState["ID"].ToString() == "")
        {
            Guid? id = null;
            PhoenixRegisterCrewPromotionConfiguration.CrewPromotionConfigurationInsert(int.Parse(ucRankFrom.SelectedRank)
            , int.Parse(ucRankTo.SelectedValue)
            , General.GetNullableString(ViewState["FLDLICENCELIST"].ToString())
            , null
            , null
            , General.GetNullableString(ViewState["FLDTASKLIST"].ToString())
            , General.GetNullableString(GetAllItemsCheckBox(lstSelectedExpinRank))
            , General.GetNullableInteger(txtRankExp.Text)
            , null
            , null
            , ref id
            , General.GetNullableInteger(txtAppraisal.Text)
           );

            ViewState["ID"] = id;
        }
        else
        {
            PhoenixRegisterCrewPromotionConfiguration.CrewPromotionConfigurationUpdate(int.Parse(ucRankFrom.SelectedRank)
                , int.Parse(ucRankTo.SelectedValue)
                , General.GetNullableString(ViewState["FLDLICENCELIST"].ToString())
                , null
                , null
                , General.GetNullableString(ViewState["FLDTASKLIST"].ToString())
                , General.GetNullableString(GetAllItemsCheckBox(lstSelectedExpinRank))
                , General.GetNullableInteger(txtRankExp.Text)
                , null
                , null
                , new Guid(ViewState["ID"].ToString())
                , General.GetNullableInteger(txtAppraisal.Text)
                );
        }

        ucStatus.Text = "Information updated.";
        BindFields();

        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1','',true);", true);

    }

    private bool IsValidPromotionConfig()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ucRankFrom.SelectedRank) == null)
            ucError.ErrorMessage = "Rank From is required.";

        if (General.GetNullableInteger(ucRankTo.SelectedValue) == null)
            ucError.ErrorMessage = "Rank To is required.";

        return (!ucError.IsError);
    }

    protected void btnExpinRankSelect_Click(object sender, EventArgs e)
    {
        string sellistfromavailable = GetSelectedItemsForCheckBoxList(lstExpinRank);
        string list = GetAllItemsCheckBox(lstSelectedExpinRank);
        list = list + "," + sellistfromavailable;
        BindRankCheckbox(lstSelectedExpinRank, 1, list);
        BindRankCheckbox(lstExpinRank, 2, list);
    }

    protected void btnExpinRankDeselect_Click(object sender, EventArgs e)
    {
        string sellistfromselected = GetSelectedItemsForCheckBoxList(lstSelectedExpinRank);
        string list = GetAllItemsCheckBox(lstExpinRank);
        list = list + "," + sellistfromselected;
        BindRankCheckbox(lstExpinRank, 1, list);
        BindRankCheckbox(lstSelectedExpinRank, 2, list);
    }

    //protected void btnExpinVTSelect_Click(object sender, EventArgs e)
    //{
    //    string sellistfromavailable = GetSelectedItems(lstExpinVesseltype);
    //    string list = GetAllItems(lstSelectedExpinVesseltype);
    //    list = list + "," + sellistfromavailable;
    //    BindVesselType(lstSelectedExpinVesseltype, 1, list);
    //    BindVesselType(lstExpinVesseltype, 2, list);
    //}

    //protected void btnExpinVTDeselect_Click(object sender, EventArgs e)
    //{
    //    string sellistfromselected = GetSelectedItems(lstSelectedExpinVesseltype);
    //    string list = GetAllItems(lstExpinVesseltype);
    //    list = list + "," + sellistfromselected;
    //    BindVesselType(lstExpinVesseltype, 1, list);
    //    BindVesselType(lstSelectedExpinVesseltype, 2, list);
    //}

    protected string GetAllItemsCheckBox(RadListBox lst)
    {
        StringBuilder strlist = new StringBuilder();
        foreach (RadListBoxItem item in lst.Items)
        {
            strlist.Append(item.Value.ToString());
            strlist.Append(",");
        }
        if (strlist.Length > 1)
        {
            strlist.Remove(strlist.Length - 1, 1);
        }
        return strlist.ToString();
    }

    protected string GetSelectedItemsForCheckBoxList(RadListBox clst)
    {
        StringBuilder strlist = new StringBuilder();
        foreach (RadListBoxItem item in clst.Items)
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

    protected void SetSelectedItemsForCheckBoxList(RadListBox clst, string list)
    {
        
        string[] arylist = list.Trim().Split(',');
        
        foreach (string item in arylist)
        {
            foreach (RadListBoxItem li in clst.Items)
            {
                if (li.Value == item)
                {
                    li.Checked = true;
                }
            }
        }
        
    }
    
}