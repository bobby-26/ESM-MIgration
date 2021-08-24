using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Text;
using System.Collections;

public partial class Registers_RegisterDocumentGlobalComponent : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        //   toolbar.AddFontAwesomeButton("../Registers/RegisterDocumentGlobalComponent.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
        MenuRankList.AccessRights = this.ViewState;
        MenuRankList.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            //Filter.CurrentDoumentCheckItem = null;
            if (Request.QueryString["DocumentCourseId"] != null && Request.QueryString["type"].ToUpper().Equals("COURSE"))
            {
                ViewState["DOCUMENTCOURSEID"] = Request.QueryString["DocumentCourseId"].ToString();
                ViewState["TYPE"] = Request.QueryString["type"];

            }
            if (Request.QueryString["DocumentLicenseId"] != null && Request.QueryString["type"].ToUpper().Equals("LICENSE"))
            {
                ViewState["DOCUMENTCOURSEID"] = Request.QueryString["DocumentLicenseId"].ToString();
                ViewState["TYPE"] = Request.QueryString["type"];

            }
            if (Request.QueryString["DocumentMedicalId"] != null && Request.QueryString["type"].ToUpper().Equals("MEDICAL"))
            {
                ViewState["DOCUMENTCOURSEID"] = Request.QueryString["DocumentMedicalId"].ToString();
                ViewState["TYPE"] = Request.QueryString["type"];

            }
            if (Request.QueryString["OtherDocumentId"] != null && Request.QueryString["type"].ToUpper().Equals("OTHER"))
            {
                ViewState["DOCUMENTCOURSEID"] = Request.QueryString["OtherDocumentId"].ToString();
                ViewState["TYPE"] = Request.QueryString["type"];

            }
        }
    }
    protected void CheckAll(Object sender, EventArgs e)
    {
        string[] ctl = Request.Form.GetValues("__EVENTTARGET");

        if (ctl != null && ctl[0].ToString() == "gvRankGroup$ctl00$ctl02$ctl00$chkAllSeal")
        {
            GridHeaderItem headerItem = gvRankGroup.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            RadCheckBox chkAll = (RadCheckBox)headerItem.FindControl("chkAllSeal");
            foreach (GridDataItem row in gvRankGroup.MasterTableView.Items)
            {
                RadCheckBox cbSelected = (RadCheckBox)row.FindControl("chkSelect");
                if (chkAll.Checked == true)
                {
                    cbSelected.Checked = true;
                }
                else
                {
                    cbSelected.Checked = false;
                }
                RadComboBox chkRankList1 = (RadComboBox)row.FindControl("chkRankList");
                if (chkRankList1 != null)
                {
                    foreach (RadComboBoxItem gvrow in chkRankList1.Items)
                    {
                        if (cbSelected.Checked == true)
                            gvrow.Checked = true;
                        else
                            gvrow.Checked = false;
                    }

                }
            }
        }
    }

    public void BindData()
    {
        DataTable dt = PhoenixRegisterDocumentRankGroup.DocumentGlobalComponentList(General.GetNullableInteger(ViewState["DOCUMENTCOURSEID"].ToString()), General.GetNullableString(txtComponentno.Text), General.GetNullableString(txtComponentname.Text));
        gvRankGroup.DataSource = dt;
    }
    protected void MenuRankList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SAVE"))
        {
            string index = "";
            string selectedmakers = "";
            //int r = 0;
            ArrayList MakerList = new ArrayList();
            foreach (GridDataItem gvrow in gvRankGroup.MasterTableView.Items)
            {
                bool result = false;

                
                if (((RadCheckBox)(gvrow.FindControl("chkSelect"))).Checked == true)
                {
                    result = true;  // ((CheckBox)gvrow.FindControl("chkSelect")).Checked;
                    //r = 1;
                }

                // Check in the Session

                if (result)
                {
                    selectedmakers = "";
                    MakerList.Clear();
                    index = gvrow.GetDataKeyValue("FLDGLOBALCOMPONENTID").ToString();
                    RadComboBox chkRankList = (RadComboBox)gvrow.FindControl("chkRankList");
                    if (chkRankList != null)
                    {
                      
                        foreach (RadComboBoxItem li in chkRankList.Items)
                        {
                            if (li.Checked)
                            {
                                MakerList.Add(li.Value.ToString());
                                //selectedmakers += li.Value + ",";
                            }
                        }
                    }


                    //if (selectedmakers != "")
                    //{
                    //    selectedmakers = "," + selectedmakers;
                    //}
                    //if(r==0)
                    //{

                    //}
                    if (MakerList != null && MakerList.Count > 0)
                    {
                        foreach (string index1 in MakerList)
                        { selectedmakers = selectedmakers + index1 + ","; }
                    }
                    if (selectedmakers != "")
                        selectedmakers = "," + selectedmakers;
                    if (ViewState["TYPE"].ToString().ToUpper().Equals("COURSE"))
                    {
                        PhoenixRegisterCrewList.DoccumentGlobalComponentInsert(Int16.Parse(ViewState["DOCUMENTCOURSEID"].ToString()), "COURSE", General.GetNullableGuid(index), selectedmakers);

                    }
                }
            }
            PhoenixRegisterCrewList.UpdateDocumentActualVesselList(int.Parse(ViewState["DOCUMENTCOURSEID"].ToString()), 1);

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('codehelp2', 'codehelp1');", true);

        }
    }
    protected void gvRankGroup_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

        BindData();

    }
    protected void SaveCheckedValues(Object sender, EventArgs e)
    {
        ArrayList SelectedSeals = new ArrayList();
        Guid index = new Guid();
        foreach (GridDataItem gvrow in gvRankGroup.MasterTableView.Items)
        {
            bool result = false;
            index = new Guid(gvRankGroup.MasterTableView.Items[0].GetDataKeyValue("FLDGLOBALCOMPONENTID").ToString());

            if (((RadCheckBox)(gvrow.FindControl("chkSelect"))).Checked == true)
            {
                result = true;// ((CheckBox)gvrow.FindControl("chkSelect")).Checked;
            }

            // Check in the Session
            //if (Filter.CurrentDoumentCheckItem != null)
            //    SelectedSeals = (ArrayList)Filter.CurrentDoumentCheckItem;
            if (result)
            {
                if (!SelectedSeals.Contains(index))
                    SelectedSeals.Add(index);
            }
            else
                SelectedSeals.Remove(index);
        }
        //if (SelectedSeals != null && SelectedSeals.Count > 0)
        //    Filter.CurrentDoumentCheckItem = SelectedSeals;
    }
    protected void gvRankGroup_ItemDataBound(object sender, GridItemEventArgs e)
    {
        RadComboBox chkRankList = (RadComboBox)e.Item.FindControl("chkRankList");
        RadLabel lblcomponent = (RadLabel)e.Item.FindControl("lblcomponent");
        RadCheckBox chkSelect = (RadCheckBox)e.Item.FindControl("chkSelect");
        RadLabel lblgroupid = (RadLabel)e.Item.FindControl("lblgroupid");


        if (chkRankList != null)
        {
            DataTable dt = new DataTable();
            dt = PhoenixRegisterDocumentRankGroup.MappedComponentWithMakerList(General.GetNullableGuid(lblgroupid.Text));

            chkRankList.DataSource = dt;
            chkRankList.DataTextField = "FLDMAKE";
            chkRankList.DataValueField = "FLDMODELID";
            chkRankList.DataBind();
            // chkRankList.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));

            General.RadBindComboBoxCheckList(chkRankList, DataBinder.Eval(e.Item.DataItem, "FLDMAKERLIST").ToString());
        }
    }

    protected void txtComponentname_TextChanged(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvRankGroup.Rebind();
    }

    protected void txtComponentno_TextChanged(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvRankGroup.Rebind();
    }

    protected void gvRankGroup_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "SELECT")
        {
            RadCheckBox chkSelect = (RadCheckBox)e.Item.FindControl("chkSelect");
            RadComboBox chkRankList1 = (RadComboBox)e.Item.FindControl("chkRankList");
            if (chkRankList1 != null)
            {
                foreach (RadComboBoxItem gvrow in chkRankList1.Items)
                {
                    if (chkSelect.Checked == true)
                        gvrow.Checked = true;
                    else
                        gvrow.Checked = false;
                }

            }
            if (chkSelect.Checked==false)
            {
                RadLabel lblgroupid = (RadLabel)e.Item.FindControl("lblgroupid");
                PhoenixRegisterDocumentRankGroup.ComponentMappingDelete(General.GetNullableInteger(ViewState["DOCUMENTCOURSEID"].ToString()),
                                                                        General.GetNullableGuid(lblgroupid.Text.ToString()));
                BindData();
                gvRankGroup.Rebind();
            }
        }
    }
}