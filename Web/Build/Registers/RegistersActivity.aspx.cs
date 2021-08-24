using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersActivity : PhoenixBasePage
{
	
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();            
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbar.AddButton("New", "NEW", ToolBarDirection.Right);
            MenuActivity.AccessRights = this.ViewState;
            MenuActivity.MenuList = toolbar.Show();

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Registers/RegistersActivity.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvActivity')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Registers/RegistersActivityFilter.aspx'); return false;", "Find", "<i class=\"fas fa-search\"></i>", "FIND");            
            MenuRegistersActivity.AccessRights = this.ViewState;
            MenuRegistersActivity.MenuList = toolbargrid.Show();
            
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["ACTIVITYID"] = null;
                BindHard();
                gvActivity.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

	protected void BindHard()		
	{
        ucHardGroup.HardTypeCode = Convert.ToInt32(PhoenixHardTypeCode.ACTIVITYGROUP).ToString();
        ucHardLeave.HardTypeCode = Convert.ToInt32(PhoenixHardTypeCode.LEAVE).ToString();
        ucHardBackToBack.HardTypeCode = Convert.ToInt32(PhoenixHardTypeCode.LEAVE).ToString();
	}


	private void Reset()
	{
		ViewState["ACTIVITYID"] = null;
		txtActivityCode.Text="";
		txtActivityName.Text = "";
		chkActive.Checked=false;
		chkPaid.Checked=false;
		chkUpdateCrewList.Checked=false;
		chkTravelFromVessel.Checked=false;
		chkSeaAllowances.Checked=false;
		txtRemarks.Text="";
		txtLevel.Text = "";
		txtPayRollHeader.Text = "";
        ucHardLeave.SelectedHard = "";
        ucHardGroup.SelectedHard = "";
        ucHardBackToBack.SelectedHard = "";
	}  

    protected void RegistersAccountMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvActivity.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

	protected void ShowExcel()
	{
		string groupid = null;
		string leaveid = null;
		DataSet ds;
		int iRowCount = 0;
		int iTotalPageCount = 0;
		string[] alColumns = { "FLDACTIVITYCODE", "FLDACTIVITYNAME", "FLDGROUP", "FLDLEAVETYPE", "FLDBACKTOBACKTYPE", "FLDPAYROLLHEADER","FLDLEVEL" };
		string[] alCaptions = { "Code", "Name", "Group", "Leave", "Back To Back", "Pay Roll Header","Level" };
		string sortexpression;
		int? sortdirection = null;

		sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

		if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
			iRowCount = 10;
		else
			iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


		if (Filter.CurrentAddressFilterCriteria != null)
		{

			NameValueCollection nvc = Filter.CurrentAddressFilterCriteria;
			if (nvc.Get("ucGroup").ToString() == "Dummy")
			{
				groupid = null;
			}
			else
			{
				groupid = nvc.Get("ucGroup").ToString();
			}
			if (nvc.Get("ucLeave").ToString() == "Dummy")
			{
				leaveid = null;
			}
			else
			{
				leaveid = nvc.Get("ucLeave").ToString();
			}			

			ds = PhoenixRegistersActivity.ActivitySearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, nvc.Get("txtActivityCode").ToString(),
							   nvc.Get("txtActivityName").ToString(), General.GetNullableInteger(groupid),
							   null, null, General.GetNullableInteger(leaveid), null, null, null, null, null,
							   nvc.Get("txtPayRollHeader").ToString(), General.GetNullableInteger(nvc.Get("txtLevel").ToString())
							   , sortexpression, sortdirection
                               , (int)ViewState["PAGENUMBER"], gvActivity.PageSize
							   , ref iRowCount, ref iTotalPageCount);
		}
		else
		{
			ds = PhoenixRegistersActivity.ActivitySearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null, null, null, null, null, null, null, null, null, null, null,
							   null, null
							   , sortexpression, sortdirection
                               , (int)ViewState["PAGENUMBER"], gvActivity.PageSize
                               , ref iRowCount, ref iTotalPageCount);
		}

		Response.AddHeader("Content-Disposition", "attachment; filename=Activity.xls");
		Response.ContentType = "application/vnd.msexcel";
		Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
		Response.Write("<td><h3>Activity</h3></td>");
		Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
		Response.Write("</tr>");
		Response.Write("</TABLE>");
		Response.Write("<br />");
		Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
		Response.Write("<tr>");
		for (int i = 0; i < alCaptions.Length; i++)
		{
			Response.Write("<td width='20%'>");
			Response.Write("<b>" + alCaptions[i] + "</b>");
			Response.Write("</td>");
		}
		Response.Write("</tr>");
		foreach (DataRow dr in ds.Tables[0].Rows)
		{
			Response.Write("<tr>");
			for (int i = 0; i < alColumns.Length; i++)
			{
				Response.Write("<td>");
				Response.Write(dr[alColumns[i]]);
				Response.Write("</td>");
			}
			Response.Write("</tr>");
		}
		Response.Write("</TABLE>");
		Response.End();
	}

    protected void Activity_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("NEW"))
            {
                ViewState["ACTIVITYID"] = null;
                Reset();
            }
            if (CommandName.ToUpper().Equals("SAVE"))
            {

                if (!IsValidActivity())
                {
                    ucError.Visible = true;
                    return;
                }

                if (ViewState["ACTIVITYID"] == null)
                {

                    PhoenixRegistersActivity.InsertActivity(PhoenixSecurityContext.CurrentSecurityContext.UserCode, txtActivityCode.Text, txtActivityName.Text, Convert.ToInt32(ucHardGroup.SelectedHard),
                                                                    chkActive.Checked == true ? 1 : 0,
                                                                    chkPaid.Checked == true ? 1 : 0,
                                                                    Convert.ToInt32(ucHardLeave.SelectedHard),
                                                                    General.GetNullableInteger(ucHardBackToBack.SelectedHard),
                                                                    chkUpdateCrewList.Checked == true ? 1 : 0,
                                                                    chkTravelFromVessel.Checked == true ? 1 : 0,
                                                                    chkSeaAllowances.Checked == true ? 1 : 0,
                                                                    txtRemarks.Text, txtPayRollHeader.Text, Convert.ToInt32(txtLevel.Text)
                                                                  );
                    Reset();
                    BindData();
                    gvActivity.Rebind();
                }
                else
                {
                    PhoenixRegistersActivity.UpdateActivity(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Convert.ToInt32(ViewState["ACTIVITYID"]), txtActivityCode.Text, txtActivityName.Text,
                                                                    Convert.ToInt32(ucHardGroup.SelectedHard),
                                                                    chkActive.Checked == true ? 1 : 0,
                                                                    chkPaid.Checked == true ? 1 : 0,
                                                                    Convert.ToInt32(ucHardLeave.SelectedHard),
                                                                    General.GetNullableInteger(ucHardBackToBack.SelectedHard),
                                                                    chkUpdateCrewList.Checked == true ? 1 : 0,
                                                                    chkTravelFromVessel.Checked == true ? 1 : 0,
                                                                    chkSeaAllowances.Checked == true ? 1 : 0,
                                                                    txtRemarks.Text, txtPayRollHeader.Text, Convert.ToInt32(txtLevel.Text)
                                                                  );
                    BindData();
                    gvActivity.Rebind();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

	public bool IsValidActivity()
	{
	   ucError.HeaderMessage = "Please provide the following required information";
       Int16 result;

	   if (txtActivityCode.Text.Trim().Equals(""))
		   ucError.ErrorMessage = "Activity Code is required.";

	   if (txtActivityName.Text.Trim().Equals(""))
		   ucError.ErrorMessage = "Activity Name is required.";

       if (ucHardGroup.SelectedHard == "" || !Int16.TryParse(ucHardGroup.SelectedHard, out result))
           ucError.ErrorMessage = "Group  is required.";

       
       if (ucHardLeave.SelectedHard == "" || !Int16.TryParse(ucHardLeave.SelectedHard, out result))
           ucError.ErrorMessage = "Leave is required.";

	   if (txtPayRollHeader.Text.Trim().Equals(""))
		   ucError.ErrorMessage = "Pay Roll Header is required.";

	   if (txtLevel.Text.Trim().Equals(""))
		   ucError.ErrorMessage = "Level is required.";
	    return (!ucError.IsError);
	}

	protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] =1;
        BindData();
        gvActivity.Rebind();
	}

    private void BindData()
    {
		string groupid = null;
		string leaveid = null;
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds;
        string[] alColumns = { "FLDACTIVITYCODE", "FLDACTIVITYNAME", "FLDGROUP", "FLDLEAVETYPE", "FLDBACKTOBACKTYPE", "FLDPAYROLLHEADER", "FLDLEVEL" };
        string[] alCaptions = { "Code", "Name", "Group", "Leave", "Back To Back", "Pay Roll Header", "Level" };
		
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
		if (Filter.CurrentAddressFilterCriteria != null)
		{
			NameValueCollection nvc = Filter.CurrentAddressFilterCriteria;
			if (nvc.Get("ucGroup").ToString() == "Dummy")
			{
				groupid = null;
			}
			else
			{
				groupid = nvc.Get("ucGroup").ToString();
			}
			if (nvc.Get("ucLeave").ToString() == "Dummy")
			{
				leaveid = null;
			}
			else
			{
				leaveid = nvc.Get("ucLeave").ToString();
			}
			

			 ds = PhoenixRegistersActivity.ActivitySearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,nvc.Get("txtActivityCode").ToString(),
								nvc.Get("txtActivityName").ToString(), General.GetNullableInteger(groupid),
								null, null, General.GetNullableInteger(leaveid), null, null, null, null, null,
								nvc.Get("txtPayRollHeader").ToString(), General.GetNullableInteger(nvc.Get("txtLevel").ToString())
								, sortexpression, sortdirection
								, (int)ViewState["PAGENUMBER"], gvActivity.PageSize
								, ref iRowCount, ref iTotalPageCount);
		}
		else
		{
			 ds = PhoenixRegistersActivity.ActivitySearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null,null, null,null, null, null, null, null, null, null, null,
								null, null
								, sortexpression, sortdirection
								, (int)ViewState["PAGENUMBER"], gvActivity.PageSize
                                , ref iRowCount, ref iTotalPageCount);
        }

        General.SetPrintOptions("gvActivity", "Activity", alCaptions, alColumns, ds);
		
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvActivity.DataSource = ds;
            gvActivity.VirtualItemCount = iRowCount;
        }
        else
        {
            gvActivity.DataSource = "";
        }
    }
    
    protected void ActivityEdit()
    {
        try
        {
            if (ViewState["ACTIVITYID"] != null)
            {

                DataSet dsActivity = PhoenixRegistersActivity.EditActivity(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                        Convert.ToInt32(ViewState["ACTIVITYID"].ToString()));

                if (dsActivity.Tables.Count > 0)
                {
                    DataRow drActivity = dsActivity.Tables[0].Rows[0];
                    txtActivityCode.Text = drActivity["FLDACTIVITYCODE"].ToString();
                    txtActivityName.Text = drActivity["FLDACTIVITYNAME"].ToString();
                    txtPayRollHeader.Text = drActivity["FLDPAYROLLHEADER"].ToString();
                    txtRemarks.Text = drActivity["FLDREMARKS"].ToString();
                    txtLevel.Text = drActivity["FLDLEVEL"].ToString();
                    ucHardGroup.SelectedHard = drActivity["FLDGROUPID"].ToString();
                    ucHardLeave.SelectedHard = drActivity["FLDLEAVETYPE"].ToString();
                    if (drActivity["FLDBACKTOBACKTYPE"].ToString() == "")
                    {
                        ucHardBackToBack.SelectedHard = "";
                    }
                    else
                    {
                        ucHardBackToBack.SelectedHard = drActivity["FLDBACKTOBACKTYPE"].ToString();
                    }
                    if (drActivity["FLDLOCALACTIVEYESNO"].ToString() == "1")
                    {
                        chkActive.Checked = true;
                    }
                    if (drActivity["FLDLOCALACTIVEYESNO"].ToString() == "0")
                    {
                        chkActive.Checked = false;
                    }
                    if (drActivity["FLDPAIDYESNO"].ToString() == "1")
                    {
                        chkPaid.Checked = true;
                    }
                    if (drActivity["FLDPAIDYESNO"].ToString() == "0")
                    {
                        chkPaid.Checked = false;
                    }
                    if (drActivity["FLDUPDATECREWLISTYESNO"].ToString() == "1")
                    {
                        chkUpdateCrewList.Checked = true;
                    }
                    if (drActivity["FLDUPDATECREWLISTYESNO"].ToString() == "0")
                    {
                        chkUpdateCrewList.Checked = false;
                    }
                    if (drActivity["FLDUPDATECREWLISTYESNO"].ToString() == "1")
                    {
                        chkUpdateCrewList.Checked = true;
                    }
                    if (drActivity["FLDUPDATECREWLISTYESNO"].ToString() == "0")
                    {
                        chkUpdateCrewList.Checked = false;
                    }
                    if (drActivity["FLDSEAALLOWANCESYESNO"].ToString() == "1")
                    {
                        chkSeaAllowances.Checked = true;
                    }
                    if (drActivity["FLDSEAALLOWANCESYESNO"].ToString() == "0")
                    {
                        chkSeaAllowances.Checked = false;
                    }
                    if (drActivity["FLDTRAVELFROMVESSELYESNO"].ToString() == "1")
                    {
                        chkTravelFromVessel.Checked = true;
                    }
                    if (drActivity["FLDTRAVELFROMVESSELYESNO"].ToString() == "0")
                    {
                        chkTravelFromVessel.Checked = false;
                    }

                    BindData();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvActivity_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "EDIT")
            {
                ViewState["ACTIVITYID"] = ((RadLabel)e.Item.FindControl("lblAccountid")).Text;
                ActivityEdit();
            }
            else if (e.CommandName.ToUpper() == "DELETE")
            {
                PhoenixRegistersActivity.DeleteActivity(1,
                  Convert.ToInt32(((RadLabel)e.Item.FindControl("lblAccountid")).Text));
                Reset();
                BindData();
                gvActivity.Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvActivity_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvActivity.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvActivity_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
        }
    }
}


