using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using Telerik.Web.UI;
public partial class CrewWarnList : PhoenixBasePage
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbarmain.AddButton("New", "NEW",ToolBarDirection.Right);
           
            CrewWarnListTab.AccessRights = this.ViewState;
            CrewWarnListTab.MenuList = toolbarmain.Show();

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewWarnList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvWarnList')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('Filter','','"+Session["sitepath"]+"/Crew/CrewWarnListFilter.aspx'); return false;", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewWarnList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuWarnList.AccessRights = this.ViewState;
            MenuWarnList.MenuList = toolbargrid.Show();

            if (!Page.IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["WARNLISTID"] = string.Empty;
				cblAddressType.DataSource = PhoenixRegistersAddress.ListAddress("128");
				cblAddressType.DataBindings.DataTextField = "FLDNAME";
				cblAddressType.DataBindings.DataValueField = "FLDADDRESSCODE";
				cblAddressType.DataBind();
                gvWarnList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
				
            }
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void MenuWarnList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string[] alColumns = { "FLDSTATUS", "FLDPASSPORTNO", "FLDSEAMANBOOKNO", "FLDFIRSTNAME", "FLDMIDDLENAME", "FLDLASTNAME", "FLDRANKNAME", "FLDNATIONALITYNAME" };
                string[] alCaptions = { "Status", "Passport Number", "Seamanbook Number", "First Name", "Middle Name", "Last Name", "Last Rank", "Nationality" };

                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                NameValueCollection nvc = Filter.WarnListFilter;

                DataTable dt = PhoenixCrewWarnList.SearchCrewWarnList(nvc != null ? General.GetNullableString(nvc.Get("txtName").ToString()) : null
                                                    , nvc != null ? General.GetNullableString(nvc.Get("txtPassportNo").ToString()) : null
                                                    , nvc != null ? General.GetNullableString(nvc.Get("txtCDCNo").ToString()) : null
                                                    , nvc != null ? General.GetNullableInteger(nvc.Get("ddlLastRank").ToString()) : null
                                                    , nvc != null ? General.GetNullableInteger(nvc.Get("ddlNationality").ToString()) : null
                                                    , nvc != null ? General.GetNullableString(nvc.Get("txtStatus").ToString()) : null
                                                    , sortexpression
                                                    , sortdirection
                                                    , 1
                                                    , iRowCount
                                                    , ref iRowCount
                                                    , ref iTotalPageCount);

                DataSet ds = new DataSet();
                ds.Tables.Add(dt.Copy());

                if (ds.Tables.Count > 0)
                    General.ShowExcel("Warn List", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.WarnListFilter = null;
                ViewState["PAGENUMBER"] = 1;
                gvWarnList.CurrentPageIndex = 0;
                gvWarnList.Rebind();
              
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CrewWarnListTab_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidate(txtPassport.Text,txtDateofBirth.Text, ddlNationality.SelectedNationality,txtSeamenBookNumber.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                if (string.IsNullOrEmpty(ViewState["WARNLISTID"].ToString()))
                {
                    SaveWarnlistInformation();
                    ResetFormControlValues(this);
                    ViewState["WARNLISTID"] = string.Empty;
                }
                else
                {
                    UpdateCrewPersonalInformation();                    
                }
                gvWarnList.Rebind();
            }
            else if (CommandName.ToUpper().Equals("NEW"))
            {

                ResetFormControlValues(this);
				rblPrincipalManager.SelectedValue = "1";
				foreach (ButtonListItem prinmanager in cblAddressType.Items)
					prinmanager.Selected = false;
				rblPrincipalManager.SelectedValue = "1";
				dvAddressType.Visible = false;
				ddlManager.Visible = true;
                ViewState["WARNLISTID"] = string.Empty;
             
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
	protected void PrincipalManagerClick(object sender, EventArgs e)
	{
		if (rblPrincipalManager.SelectedValue == "2")
		{
			dvAddressType.Visible = true;

			dvAddressType.Attributes["class"] = "input_mandatory";
			cblAddressType.Enabled = true;
			ddlManager.Visible = false;
		}
		else
		{
			dvAddressType.Visible = false;
			ddlManager.Visible = true;
			cblAddressType.SelectedValue = null;
			cblAddressType.Enabled = false;
			dvAddressType.Attributes["class"] = "input";
		}
	}
    public void SaveWarnlistInformation()
    {
		StringBuilder straddresstype = new StringBuilder();

		foreach (ButtonListItem item in cblAddressType.Items)
		{
			if (item.Selected == true)
			{
				straddresstype.Append(item.Value.ToString());
				straddresstype.Append(",");
			}
		}

		if (straddresstype.Length > 1)
		{
			straddresstype.Remove(straddresstype.Length - 1, 1);
		}
        PhoenixCrewWarnList.InsertCrewWarnList(txtPassport.Text, txtSeamenBookNumber.Text, txtFirstname.Text, txtLastName.Text, txtMiddleName.Text
                                              , Convert.ToInt32(ddlNationality.SelectedNationality)
                                              , General.GetNullableDateTime(txtDateofBirth.Text), General.GetNullableInteger(ddlRank.SelectedRank)
                                              , txtLastShip.Text
                                              , txtCompany.Text
                                              , txtReason.Text
                                              , "WARNLIST"
											  , rblPrincipalManager.SelectedValue.ToString() == "1" ? ddlManager.SelectedAddress.ToString() : straddresstype.ToString());
        ucStatus.Text = "Warnlist Information Updated";
    }
    private bool IsValidate(string passportnumber,string dateofbirth, string nationality,string cdcno)
    {
        DateTime resultdate;
        int iNationality;
        ucError.HeaderMessage = "Please provide the following required information";
       
        if (passportnumber.Trim() == "")
        {
            ucError.ErrorMessage = "Passport Number cannot be blank";
        }
		if (cdcno.Trim() == "")
		{
			ucError.ErrorMessage = "Seaman's Book Number cannot be blank";
		}
        if (!string.IsNullOrEmpty(dateofbirth) && DateTime.TryParse(dateofbirth, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Date of Birth should be earlier than current date";
        }
        
        if (nationality.Trim().Equals("Dummy"))
        {
            if (Int32.TryParse(nationality, out iNationality) == false)
                ucError.ErrorMessage = "Select the Nationality.";
        }
		if (rblPrincipalManager.SelectedValue == "1")
		{
			if (General.GetNullableInteger(ddlManager.SelectedAddress)==null || General.GetNullableInteger(ddlManager.SelectedAddress)==null)
				ucError.ErrorMessage = "Please select a Manager";
		}
		else
		{
			if (General.GetNullableInteger(cblAddressType.SelectedValue) == null)
				ucError.ErrorMessage = "Please select a Principal";
		}
        return (!ucError.IsError);
    }
   
    public void UpdateCrewPersonalInformation()
    {
		StringBuilder straddresstype = new StringBuilder();

		foreach (ButtonListItem item in cblAddressType.Items)
		{
			if (item.Selected == true)
			{
				straddresstype.Append(item.Value.ToString());
				straddresstype.Append(",");
			}
		}

		if (straddresstype.Length > 1)
		{
			straddresstype.Remove(straddresstype.Length - 1, 1);
		}
        PhoenixCrewWarnList.UpdateCrewWarnList(int.Parse(ViewState["WARNLISTID"].ToString()),txtPassport.Text, txtSeamenBookNumber.Text, txtFirstname.Text, txtLastName.Text, txtMiddleName.Text
                                             , Convert.ToInt32(ddlNationality.SelectedNationality)
                                             , General.GetNullableDateTime(txtDateofBirth.Text), General.GetNullableInteger(ddlRank.SelectedRank)
                                             , txtLastShip.Text
                                             , txtCompany.Text
                                             , txtReason.Text
											 , rblPrincipalManager.SelectedValue.ToString() == "1" ? ddlManager.SelectedAddress.ToString() : straddresstype.ToString());
        ucStatus.Text = "Warnlist Information Updated";
    }

    protected void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSTATUS", "FLDPASSPORTNO", "FLDSEAMANBOOKNO", "FLDFIRSTNAME", "FLDMIDDLENAME", "FLDLASTNAME", "FLDRANKNAME", "FLDNATIONALITYNAME" };
        string[] alCaptions = { "Status", "Passport Number", "Seamanbook Number", "First Name", "Middle Name", "Last Name", "Last Rank", "Nationality" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        else
            sortdirection = 1; //defaulting descending order for Relief due date
        try
        {
            NameValueCollection nvc = Filter.WarnListFilter;

            DataTable dt = PhoenixCrewWarnList.SearchCrewWarnList(nvc != null ? General.GetNullableString(nvc.Get("txtName").ToString()) : null
                                                , nvc != null ? General.GetNullableString(nvc.Get("txtPassportNo").ToString()) : null
                                                , nvc != null ? General.GetNullableString(nvc.Get("txtCDCNo").ToString()) : null
                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ddlLastRank").ToString()) : null
                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ddlNationality").ToString()) : null
                                                , nvc != null ? General.GetNullableString(nvc.Get("txtStatus").ToString()) : null
                                                , sortexpression
                                                , sortdirection
                                                , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                , gvWarnList.PageSize
                                                , ref iRowCount
                                                , ref iTotalPageCount);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());

            General.SetPrintOptions("gvWarnList", "Warn List", alCaptions, alColumns, ds);
            gvWarnList.DataSource = dt;
            gvWarnList.VirtualItemCount = iRowCount;
          
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SetWarnListInformation(int iWarnList)
    {
        DataTable dt = PhoenixCrewWarnList.ListCrewWarnList(iWarnList);
        if (dt.Rows.Count > 0)
        {
            txtPassport.Text = dt.Rows[0]["FLDPASSPORTNO"].ToString();
            txtSeamenBookNumber.Text = dt.Rows[0]["FLDSEAMANBOOKNO"].ToString();
            txtFirstname.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
            txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
            txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
            ddlNationality.SelectedNationality = dt.Rows[0]["FLDNATIONALITY"].ToString();
            txtDateofBirth.Text = General.GetDateTimeToString(dt.Rows[0]["FLDDATEOFBIRTH"].ToString());
            ddlRank.SelectedRank = dt.Rows[0]["FLDLASTRANK"].ToString();
            txtLastShip.Text = dt.Rows[0]["FLDLASTSHIP"].ToString();
            txtCompany.Text = dt.Rows[0]["FLDCOMPANY"].ToString();
            txtReason.Text = dt.Rows[0]["FLDREASON"].ToString();

            if (dt.Rows[0]["FLDMANAGERYN"].ToString() == "1")
            {
                rblPrincipalManager.SelectedValue = "1";
                dvAddressType.Visible = false;
                ddlManager.Visible = true;
                ddlManager.SelectedAddress = dt.Rows[0]["FLDPRINCIPLEMANGERID"].ToString();
            }
            if (dt.Rows[0]["FLDMANAGERYN"].ToString() == "2") //else if (dt.Rows[0]["FLDPRINCIPLEMANGERID"].ToString().Contains(","))
            {
                rblPrincipalManager.SelectedValue = "2";
                dvAddressType.Visible = true;
                ddlManager.Visible = false;
                string[] addresstype = dt.Rows[0]["FLDPRINCIPLEMANGERID"].ToString().Split(',');
                foreach (string item in addresstype)
                {
                    if (item.Trim() != "")
                    {
                        cblAddressType.SelectedValue=item;
                    }
                }

            }
            else if (dt.Rows[0]["FLDMANAGERYN"].ToString() == "")
            {
                rblPrincipalManager.SelectedValue = "1";
                dvAddressType.Visible = false;
                ddlManager.Visible = true;
            }
            
        }
    }
      
    private void ResetFormControlValues(Control parent)
    {
        try
        {
            txtPassport.Text = "";
            txtSeamenBookNumber.Text = "";
            txtFirstname.Text = "";
            txtMiddleName.Text = "";
            txtLastName.Text = "";
            ddlNationality.SelectedNationality = "";
            txtDateofBirth.Text = "";
            ddlRank.SelectedRank = "";
            txtLastShip.Text = "";
            txtCompany.Text = "";
            txtReason.Text = "";
            rblPrincipalManager.SelectedIndex = 0;
            dvAddressType.Visible = false;
            ddlManager.Visible = true;
            ddlManager.SelectedAddress = "";
            cblAddressType.SelectedItems.Clear();

          

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
       
        gvWarnList.Rebind();
       
    }

    protected void gvWarnList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvWarnList.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvWarnList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "DELETE")
        {
            try
            {

               
                string strWarnListId = ((RadLabel)e.Item.FindControl("lblWarnListId")).Text;
                PhoenixCrewWarnList.DeleteCrewWarnList(int.Parse(strWarnListId));
                gvWarnList.Rebind();
              
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;

            }
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
        else if(e.CommandName.ToUpper()== "EDIT")
        {
            try
            {
                string strWarnListId = ((RadLabel)e.Item.FindControl("lblWarnListId")).Text;
                SetWarnListInformation(int.Parse(strWarnListId));
                ViewState["WARNLISTID"] = strWarnListId;
                e.Item.Selected = true;
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
    }

    protected void gvWarnList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        
        if (e.Item is GridDataItem)
        {
            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);


            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
           
        }
      
    }

    protected void gvWarnList_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression.Replace(" ASC", "").Replace(" DESC", "");
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
}
