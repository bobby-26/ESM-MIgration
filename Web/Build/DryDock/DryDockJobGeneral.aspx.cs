using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.DryDock;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;


public partial class DryDockJobGeneral : PhoenixBasePage
{

	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
			            
			PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("List", "LIST");
            toolbar.AddButton("Details", "DETAIL");
            toolbar.AddButton("Supt Remarks", "SUPTREMARKS");
            toolbar.AddButton("Work Requests", "WORKREQUESTS");
            toolbar.AddButton("Attachment", "ATTACHMENT");
            
            MenuHeader.AccessRights = this.ViewState;
            MenuHeader.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
            
            toolbar.AddLinkButton("javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/DryDock/DryDockJobDiff.aspx?jobid=" + Request.QueryString["STANDARDJOBID"] + "')", "History", "HISTORY");
            MenuStandardJobSpecification.AccessRights = this.ViewState;
            MenuStandardJobSpecification.MenuList = toolbar.Show();

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../DryDock/DryDockJobGeneral.aspx?STANDARDJOBID=" + Request.QueryString["STANDARDJOBID"] + "&pno=" + Request.QueryString["pno"], "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
			toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvStandardJobLineItem')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
			MenuDryDockStandardJobLineItem.AccessRights = this.ViewState;
			MenuDryDockStandardJobLineItem.MenuList = toolbargrid.Show();

			if (!IsPostBack)
			{               
				ViewState["STANDARDJOBID"] = null;
				if (Request.QueryString["STANDARDJOBID"] != null)
				{
					ViewState["STANDARDJOBID"] = Request.QueryString["STANDARDJOBID"];
				}
								
				MenuHeader.SelectedMenuIndex = 1;
				BindCheckBox();
				BindFields(new Guid(Request.QueryString["STANDARDJOBID"]));
				ViewState["PAGENUMBER"] = 1;
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
			}
			BindData();
            BindDataComponent();
            BindAdditionalJobData();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void DryDockStandardJobLineItem_TabStripCommand(object sender, EventArgs e)
	{
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("EXCEL"))
		{
			ShowExcel();
		}
	}

	protected void ShowExcel()
	{
		try
		{
			string[] alColumns = { "FLDDESCRIPTION", "FLDUNITNAME" };
			string[] alCaptions = { "Description", "Unit" };

			int? sortdirection = null;

			DataSet ds = PhoenixDryDockJobGeneral.ListDryDockJobGeneralDetail(
			General.GetNullableGuid(ViewState["STANDARDJOBID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);

			General.ShowExcel("Standard Job Line Item", ds.Tables[0], alColumns, alCaptions, sortdirection, null);
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	private void BindFields(Guid? standardjobid)
	{
		try
		{
            if ((standardjobid != null))
            {
                DataSet ds = PhoenixDryDockJobGeneral.EditDryDockJobGeneral(standardjobid, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                DataRow dr = ds.Tables[0].Rows[0];
                //txtComponentName.Text = dr["FLDCOMPONENTNAME"].ToString();
                //txtComponentCode.Text = dr["FLDCOMPONENTNUMBER"].ToString();
                //txtComponentId.Text = dr["FLDCOMPONENTID"].ToString();
                //txtTechnicalSpec.Text = Html2Text(dr["FLDSPECIFICATION"].ToString());
                txtLocation.Text = dr["FLDLOCATION"].ToString();
                txtNumber.Text = dr["FLDNUMBER"].ToString();
                txtTitle.Text = dr["FLDTITLE"].ToString();

                txtDuration.Text = dr["FLDESTIMATEDHOURS"].ToString();

                if (dr["FLDESTIMATEDHOURS"].ToString() != "")
                    txtDuration.Text = Math.Round(Convert.ToDecimal(txtDuration.Text)).ToString();

                ddlJobType.SelectedValue = dr["FLDJOBTYPE"].ToString();
                txtJobDescription.Text = General.SanitizeHtml(dr["FLDJOBDESCRIPTION"].ToString());
                txtVesselJobDescription.Text = General.SanitizeHtml(dr["FLDVESSELJOBDESCRIPTION"].ToString());
                txtOfficeJobDescription.Text = General.SanitizeHtml(dr["FLDOFFICEJOBDESCRIPTION"].ToString());
                lblTempVesselJobDescription.Text = General.SanitizeHtml(dr["FLDVESSELJOBDESCRIPTION"].ToString());
                lblTempOfficeJobDescription.Text = General.SanitizeHtml(dr["FLDOFFICEJOBDESCRIPTION"].ToString());
                BindCheckBoxList(cblEnclosed, dr["FLDENCLOSED"].ToString());
                BindCheckBoxList(cblMaterial, dr["FLDMATERIAL"].ToString());
                BindCheckBoxList(cblWorkSurvey, dr["FLDWORKSURVEYBY"].ToString());
                BindCheckBoxList(cblInclude, dr["FLDINCLUDE"].ToString());
                BindCheckBoxList(cblResponsibilty, dr["FLDRESPONSIBILITY"].ToString());
                if (dr["FLDESTIMATEDDAYS"].ToString() != string.Empty)
                    rblEstimatedDays.SelectedValue = dr["FLDESTIMATEDDAYS"].ToString();
                if (dr["FLDCOSTCLASSIFICATION"].ToString() != string.Empty)
                    ddlCostClassification.SelectedValue = dr["FLDCOSTCLASSIFICATION"].ToString();
                if (dr["FLDBUDGECODEID"].ToString() != string.Empty)
                    radddlbudgetcode.SelectedValue = dr["FLDBUDGECODEID"].ToString();
                ViewState["DTKEY"] = dr["FLDDTKEY"].ToString();
                ViewState["OPERATIONMODE"] = "EDIT";
                //PhoenixDryDockJobGeneral.UpdateDryDockJobGeneralChange(PhoenixSecurityContext.CurrentSecurityContext.VesselID, standardjobid.Value);
            }
            else
            {
                ViewState["OPERATIONMODE"] = "ADD";
            }

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
    private string Html2Text(string strHTML)
    {
        string HTMLText = string.Empty;
        try
        {
            strHTML = "<div>" + HttpUtility.HtmlDecode(strHTML) + "</div";
            Sgml.SgmlReader xhtml = new Sgml.SgmlReader();
            StringWriter sw = new StringWriter();
            XmlTextWriter writer = new XmlTextWriter(sw);
            xhtml.DocType = "HTML";
            xhtml.InputStream = new StringReader(strHTML);

            while (!xhtml.EOF)
                writer.WriteNode(xhtml, true);
            writer.Close();

            strHTML = sw.ToString().Replace("&amp;", "&#38;").Replace("&nbsp;", "&#160;").Replace("&quot;", "'");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(strHTML);
            HTMLText =  doc.InnerText;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        return HTMLText;
    }

	protected void BindCheckBox()
	{
		string type = string.Empty;

		cblEnclosed.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(4);
		cblEnclosed.DataTextField = "FLDNAME";
		cblEnclosed.DataValueField = "FLDMULTISPECID";
		cblEnclosed.DataBind();

		cblMaterial.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(1);
		cblMaterial.DataTextField = "FLDNAME";
		cblMaterial.DataValueField = "FLDMULTISPECID";
		cblMaterial.DataBind();

        cblInclude.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(3);
        cblInclude.DataTextField = "FLDNAME";
        cblInclude.DataValueField = "FLDMULTISPECID";
        cblInclude.DataBind();


        cblWorkSurvey.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(2);
        cblWorkSurvey.DataTextField = "FLDNAME";
        cblWorkSurvey.DataValueField = "FLDMULTISPECID";
        cblWorkSurvey.DataBind();
       
        cblResponsibilty.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(5);
        cblResponsibilty.DataTextField = "FLDNAME";
        cblResponsibilty.DataValueField = "FLDMULTISPECID";
        cblResponsibilty.DataBind();

        ddlJobType.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(7);
        ddlJobType.DataTextField = "FLDNAME";
        ddlJobType.DataValueField = "FLDMULTISPECID";
        ddlJobType.DataBind();

        ddlCostClassification.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(8);
        ddlCostClassification.DataTextField = "FLDNAME";
        ddlCostClassification.DataValueField = "FLDMULTISPECID";
        ddlCostClassification.DataBind();

        rblEstimatedDays.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(9);
        rblEstimatedDays.DataBindings.DataTextField = "FLDNAME";
        rblEstimatedDays.DataBindings.DataValueField = "FLDMULTISPECID";
        rblEstimatedDays.DataBind();

        radddlbudgetcode.DataSource = PhoenixDryDockOrder.DrydockBudgetcodeList(null);
        radddlbudgetcode.DataTextField = "FLDBUDGETCODE";
        radddlbudgetcode.DataValueField = "FLDBUDGETID";
        radddlbudgetcode.DataBind();

       
    }
    protected void StandardJobSpecification_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidRepairSpec())
                {
                    ucError.Visible = true;
                    return;
                }

                string strMat = ReadCheckBoxList(cblMaterial);
                string strEnc = ReadCheckBoxList(cblEnclosed);
                string strWrk = ReadCheckBoxList(cblWorkSurvey);
                string strInc = ReadCheckBoxList(cblInclude);
                string strEst = rblEstimatedDays.SelectedValue;
                string strRes = ReadCheckBoxList(cblResponsibilty);

                PhoenixDryDockJobGeneral.UpdateDryDockJobGeneral(
                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                    new Guid(ViewState["STANDARDJOBID"].ToString()),
                                                    2,//standardjob
                                                    General.GetNullableString(txtNumber.Text).Trim(),
                                                    txtTitle.Text.Trim(),
                                                    strRes,
                                                    General.GetNullableDecimal(txtDuration.Text),
                                                    null,
                                                    General.GetNullableInteger(ddlJobType.SelectedValue),
                                                    General.GetNullableString(txtJobDescription.Text).Trim(),
                                                    General.GetNullableString(txtLocation.Text),
                                                    strWrk, strInc, strMat, strEnc, General.GetNullableInteger(ddlCostClassification.SelectedValue), strEst
                                                    );
                //if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                {
                    PhoenixDryDockJobGeneral.UpdateDryDockJobGeneralVesselSpecification(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                        , new Guid(ViewState["STANDARDJOBID"].ToString())
                        , General.GetNullableString(txtVesselJobDescription.Text)
                        , General.GetNullableString(txtOfficeJobDescription.Text)
                        );
                }
                PhoenixCommonDiffUtil.Item[] diffVessel = PhoenixCommonDiffUtil.DiffText(lblTempVesselJobDescription.Text, txtVesselJobDescription.Text, false, true, false);
                PhoenixCommonDiffUtil.Item[] diffOffice = PhoenixCommonDiffUtil.DiffText(lblTempOfficeJobDescription.Text, txtOfficeJobDescription.Text, false, true, false);
                if (diffVessel.Length > 0 && PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                {
                    PhoenixDryDockJob.InsertDryDockJobDescriptionChange(General.GetNullableGuid(ViewState["STANDARDJOBID"].ToString()).Value
                        , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                        , PhoenixCommonDiffUtil.DiffReport(diffVessel, lblTempVesselJobDescription.Text, txtVesselJobDescription.Text)
                    );
                }
                if (diffOffice.Length > 0 && PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                {
                    PhoenixDryDockJob.InsertDryDockJobDescriptionChange(General.GetNullableGuid(ViewState["STANDARDJOBID"].ToString()).Value
                        , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                        , PhoenixCommonDiffUtil.DiffReport(diffOffice, lblTempOfficeJobDescription.Text, txtOfficeJobDescription.Text)
                    );
                }
                //}                

                PhoenixDryDockJobGeneral.DrydockJobGenralBudgetcodeInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                  , General.GetNullableGuid(ViewState["STANDARDJOBID"].ToString())
                  , General.GetNullableInteger(radddlbudgetcode.SelectedValue));

                ucStatus.Text = "Details Updated.";
                BindFields(new Guid(ViewState["STANDARDJOBID"].ToString()));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindCheckBoxList(RadListBox cbl, string list)
    {
        foreach (RadListBoxItem li in cbl.Items)
        {
            li.Checked = false;
        }
        foreach (string item in list.Split(','))
        {
            if (item.Trim() != "")
            {
                if (cbl.FindItemByValue(item) != null)
                    cbl.FindItemByValue(item).Checked = true;
            }
        }


     
    }

    private string ReadCheckBoxList(RadListBox cbl)
    {
        string list = string.Empty;

        foreach (RadListBoxItem item in cbl.Items)
        {
            if (item.Checked == true)
            {
                list = list + item.Value.ToString() + ",";
            }
        }
        list = list.TrimEnd(',');
        return list;
    }    
	private bool IsValidRepairSpec()
	{

		ucError.HeaderMessage = "Please provide the following required information.";

		if (txtNumber.Text.Trim().Equals(""))
			ucError.ErrorMessage = "Number cannot be blank.";

		if (txtTitle.Text.Trim().Equals(""))
			ucError.ErrorMessage = "Title cannot be blank.";

		if (txtJobDescription.Text.Trim().Equals(""))
			ucError.ErrorMessage = "Job Description cannot be blank.";

		if (General.GetNullableInteger(ddlJobType.SelectedValue) == null)
			ucError.ErrorMessage = "Job Type cannot be blank.";

        if (General.GetNullableInteger(ddlCostClassification.SelectedValue) == null)
            ucError.ErrorMessage = "Cost Classification is required";

		return (!ucError.IsError);
	}
	protected void MenuHeader_TabStripCommand(object sender, EventArgs e)
	{
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("DETAIL"))
		{
            Response.Redirect("../DryDock/DryDockJobGeneral.aspx?STANDARDJOBID=" + ViewState["STANDARDJOBID"].ToString() + "&pno=" + Request.QueryString["pno"], false);
		}
		else if (CommandName.ToUpper().Equals("LIST"))
		{
            Response.Redirect("../DryDock/DryDockJobGeneralList.aspx?STANDARDJOBID=" + ViewState["STANDARDJOBID"].ToString() + "&pno=" + Request.QueryString["pno"], false);
		}
        else if (CommandName.ToUpper().Equals("ATTACHMENT"))
        {
            Response.Redirect("../DryDock/DryDockJobAttachments.aspx?STANDARDJOBID=" + ViewState["STANDARDJOBID"].ToString() + "&DTKEY=" + ViewState["DTKEY"].ToString() + "&MOD=" + PhoenixModule.DRYDOCK + "&pno=" + Request.QueryString["pno"], false);
       }
        else if (CommandName.ToUpper().Equals("WORKREQUESTS"))
        {
            Response.Redirect("../DryDock/DryDockStandardJobWorkRequest.aspx?STANDARDJOBID=" + ViewState["STANDARDJOBID"].ToString() + "&pno=" + Request.QueryString["pno"]);
        }
        else if (CommandName.ToUpper().Equals("SUPTREMARKS"))
        {
            Response.Redirect("../DryDock/DryDockDiscussion.aspx?STANDARDJOBID=" + ViewState["STANDARDJOBID"].ToString() + "&pno=" + Request.QueryString["pno"] + "&DTKEY=" + ViewState["DTKEY"].ToString());
        }
	}

	private void BindData()
	{


        string[] alColumns = { "FLDDESCRIPTION", "FLDUNITNAME",};
		string[] alCaptions = { "Job Detail", "Unit",};

		DataSet ds = PhoenixDryDockJobGeneral.ListDryDockJobGeneralDetail(
            General.GetNullableGuid(ViewState["STANDARDJOBID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);

		General.SetPrintOptions("gvStandardJobLineItem", "Standard Job line Item", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvStandardJobLineItem.PageSize = ds.Tables[0].Rows.Count;
        }
       
        gvStandardJobLineItem.DataSource = ds;
        

    }
    private void BindAdditionalJobData()
    {


        string[] alColumns = { "FLDSERIALNO", "FLDDETAIL", "FLDUNIT" };
        string[] alCaptions = { "Serial No", "Job Detail", "Unit" };

        DataTable dt = PhoenixDryDockJobGeneral.ListDryDockJobGeneralAdditionalJob(
            General.GetNullableGuid(ViewState["STANDARDJOBID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.SetPrintOptions("gvAddDetail", "Additional Job line Item", alCaptions, alColumns, ds);
        if (dt.Rows.Count > 0)
        {
            gvAddDetail.PageSize = dt.Rows.Count;
        }
       
        gvAddDetail.DataSource = dt;
      

    }
    private void BindDataComponent()
    {


        if (General.GetNullableGuid(ViewState["STANDARDJOBID"].ToString()).HasValue)
        {
            DataTable dt = PhoenixDryDockJob.ListDryDockJobComponent(General.GetNullableGuid(ViewState["STANDARDJOBID"].ToString()).Value, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            if (dt.Rows.Count > 0)
            {
                gvComponent.PageSize = dt.Rows.Count;
            }
           
            gvComponent.DataSource = dt;
           // gvComponent.DataBind();
           
        }
    }

	protected void cmdSort_Click(object sender, EventArgs e)
	{
		ImageButton ib = (ImageButton)sender;

		ViewState["SORTEXPRESSION"] = ib.CommandName;
		ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

	}

	protected void gvStandardJobLineItem_RowCreated(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow
			&& e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
			&& e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
		{
			e.Row.TabIndex = -1;
			e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvStandardJobLineItem, "Select$" + e.Row.RowIndex.ToString(), false);
		}
	}

	protected void gvStandardJobLineItem_Sorting(object sender, GridViewSortEventArgs se)
	{
		//gvStandardJobLineItem.SelectedIndex = -1;
		//gvStandardJobLineItem.EditIndex = -1;

		ViewState["SORTEXPRESSION"] = se.SortExpression;

		if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
			ViewState["SORTDIRECTION"] = 1;
		else
			ViewState["SORTDIRECTION"] = 0;

		BindData();
	}

	//protected void gvStandardJobLineItem_RowUpdating(object sender, GridViewUpdateEventArgs e)
	//{
	//	try
	//	{
	//		GridView _gridView = (GridView)sender;
	//		int nCurrentRow = e.RowIndex;
	//		if (!IsValidRepairJobDetail(ViewState["STANDARDJOBID"].ToString(),
	//					((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDetailEdit")).Text))
	//		{
	//			ucError.Visible = true;
	//			return;
	//		}

	//		PhoenixDryDockJobGeneral.UpdateDryDockJobGeneralDetail(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
	//			 new Guid(((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lbljobdetailidEdit")).Text),
	//			 new Guid(ViewState["STANDARDJOBID"].ToString()),
	//			 ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDetailEdit")).Text,
	//			General.GetNullableInteger(((UserControlUnit)_gridView.Rows[nCurrentRow].FindControl("ucUnitEdit")).SelectedUnit));



	//		_gridView.EditIndex = -1;
	//		BindData();

	//	}
	//	catch (Exception ex)
	//	{
	//		ucError.ErrorMessage = ex.Message;
	//		ucError.Visible = true;
	//	}
	//}

	

	protected void gvStandardJobLineItem_RowEditing(object sender, GridViewEditEventArgs de)
	{
		try
		{
			GridView _gridView = (GridView)sender;

			_gridView.EditIndex = de.NewEditIndex;
			_gridView.SelectedIndex = de.NewEditIndex;

			BindData();

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	//protected void gvStandardJobLineItem_RowCommand(object sender, GridViewCommandEventArgs e)
	//{
	//	try
	//	{
	//		if (e.CommandName.ToUpper().Equals("SORT"))
	//			return;

	//		GridView _gridView = (GridView)sender;
	//		int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

	//		if (e.CommandName.ToUpper().Equals("ADD"))
	//		{
	//			if (!IsValidRepairJobDetail(ViewState["STANDARDJOBID"].ToString(),
	//				((TextBox)_gridView.FooterRow.FindControl("txtDetailAdd")).Text))
	//			{
	//				ucError.Visible = true;
	//				return;
	//			}
	//			Guid? jobdetailid = null;
	//			jobdetailid = PhoenixDryDockJobGeneral.InsertDryDockJobGeneralDetail(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
	//			 new Guid(ViewState["STANDARDJOBID"].ToString()),
	//			 ((TextBox)_gridView.FooterRow.FindControl("txtDetailAdd")).Text,
	//			General.GetNullableInteger(((UserControlUnit)_gridView.FooterRow.FindControl("ucUnitAdd")).SelectedUnit),
	//			ref jobdetailid);

	//			BindData();

	//		}

 //           if (e.CommandName.ToUpper().Equals("MOVEUP"))
 //           {

 //               PhoenixDryDockJobGeneral.MoveDryDockJobGeneralDetailUpOrDown(PhoenixSecurityContext.CurrentSecurityContext.UserCode
 //                   , new Guid(ViewState["STANDARDJOBID"].ToString())
 //                   ,General.GetNullableGuid(((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblDTKey")).Text), 1 // UP
 //                   );

 //               BindData();
 //           }

 //           if (e.CommandName.ToUpper().Equals("MOVEDOWN"))
 //           {
 //               PhoenixDryDockJobGeneral.MoveDryDockJobGeneralDetailUpOrDown(PhoenixSecurityContext.CurrentSecurityContext.UserCode
 //                   , new Guid(ViewState["STANDARDJOBID"].ToString())
 //                   , General.GetNullableGuid(((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblDTKey")).Text), -1 // DOWN
 //                   );

 //               BindData();
 //           }


	//	}
	//	catch (Exception ex)
	//	{
	//		ucError.ErrorMessage = ex.Message;
	//		ucError.Visible = true;
	//	}
	//}

	//protected void gvStandardJobLineItem_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
	//{
	//	if (e.Row.RowType == DataControlRowType.Header)
	//	{
	//		if (ViewState["SORTEXPRESSION"] != null)
	//		{
	//			HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
	//			if (img != null)
	//			{
	//				if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
	//					img.Src = Session["images"] + "/arrowUp.png";
	//				else
	//					img.Src = Session["images"] + "/arrowDown.png";

	//				img.Visible = true;
	//			}
	//		}
	//	}

	//	if (e.Row.RowType == DataControlRowType.Footer)
	//	{
	//		ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
	//		if (db != null)
	//		{
	//			if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
	//				db.Visible = false;
	//		}
	//	}

	//	if (e.Row.RowType == DataControlRowType.DataRow)
	//	{
			
	//		ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
	//		if (edit != null)
	//		{
	//			edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
	//		}

	//		ImageButton save = (ImageButton)e.Row.FindControl("cmdSave");
	//		if (save != null)
	//		{
	//			save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);
	//		}

	//		ImageButton cancel = (ImageButton)e.Row.FindControl("cmdCancel");
	//		if (cancel != null)
	//		{
	//			cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);
	//		}

 //           ImageButton mup = (ImageButton)e.Row.FindControl("cmdMoveUp");
 //           if (mup != null)
 //           {
 //               mup.Visible = SessionUtil.CanAccess(this.ViewState, mup.CommandName);
 //           }

 //           ImageButton mdwn = (ImageButton)e.Row.FindControl("cmdMoveDown");
 //           if (mdwn != null)
 //           {
 //               mdwn.Visible = SessionUtil.CanAccess(this.ViewState, mdwn.CommandName);
 //           }

	//		ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
	//		if (db != null)
	//		{
	//			db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
 //               db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
	//		}
	//		UserControlUnit ucUnit = (UserControlUnit)e.Row.FindControl("ucUnitEdit");            
	//		DataRowView drv = (DataRowView)e.Row.DataItem;
	//		if (ucUnit != null) ucUnit.SelectedUnit = drv["FLDUNIT"].ToString();
 //           CheckBox cb = (CheckBox)e.Row.FindControl("chkSelectedYN");
 //           cb.Enabled = SessionUtil.CanAccess(this.ViewState, "SELECTDETAIL");
 //           Button b = (Button)e.Row.FindControl("cmdSelectedYN");
 //           string jvscript = "";
 //           if (b != null) jvscript = "javascript:selectJobDetail('" + drv["FLDJOBDETAILID"].ToString() + "',this);";
 //           if (cb != null && b != null) { cb.Attributes.Add("onclick", jvscript); cb.Checked = drv["FLDSELECTEDYN"].ToString().Equals("0") ? false : true; }
 //           if (b != null) b.Attributes.Add("style", "visibility:hidden");
	//	}
	//}

    //protected void gvStandardJobLineItem_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;

    //        PhoenixDryDockJobGeneral.DeleteDryDockJobGeneralDetail(new Guid(((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lbljobdetailid")).Text),
    //         new Guid(ViewState["STANDARDJOBID"].ToString()));

    //        _gridView.EditIndex = -1;
    //        BindData();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

	protected void gvStandardJobLineItem_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		_gridView.EditIndex = -1;
		BindData();
	}

	protected void cmdSearch_Click(object sender, EventArgs e)
	{
		//gvStandardJobLineItem.SelectedIndex = -1;
		//gvStandardJobLineItem.EditIndex = -1;
		gvStandardJobLineItem.Rebind();

	}
    //protected void gvComponent_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;
    //        string componentid = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtComponentId")).Text;
    //        string description = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDescription")).Text;
    //        Guid id = (Guid)_gridView.DataKeys[nCurrentRow].Value;
    //        if (!IsValidComponent(componentid))
    //        {
    //            ucError.Visible = true;
    //            return;
    //        }

    //        PhoenixDryDockJob.UpdateDryDockJobComponent(
    //        General.GetNullableGuid(ViewState["STANDARDJOBID"].ToString()),
    //        General.GetNullableGuid(componentid),
    //        2,
    //        PhoenixSecurityContext.CurrentSecurityContext.VesselID, id, description);

    //        _gridView.EditIndex = -1;
    //        BindDataComponent();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void gvComponent_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            BindDataComponent();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void gvComponent_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        if (e.CommandName.ToUpper().Equals("SORT"))
    //            return;

    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

    //        if (e.CommandName.ToUpper().Equals("ADD"))
    //        {
    //            string componentid = ((TextBox)_gridView.FooterRow.FindControl("txtComponentIdAdd")).Text;                
    //            if (!IsValidComponent(componentid))
    //            {
    //                ucError.Visible = true;
    //                return;
    //            }

    //            PhoenixDryDockJob.UpdateDryDockJobComponent(
    //            General.GetNullableGuid(ViewState["STANDARDJOBID"].ToString()),
    //            General.GetNullableGuid(componentid),
    //            2,
    //            PhoenixSecurityContext.CurrentSecurityContext.VesselID, null);

    //            BindDataComponent();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void gvComponent_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {
    //        if (ViewState["SORTEXPRESSION"] != null)
    //        {
    //            HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
    //            if (img != null)
    //            {
    //                if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
    //                    img.Src = Session["images"] + "/arrowUp.png";
    //                else
    //                    img.Src = Session["images"] + "/arrowDown.png";

    //                img.Visible = true;
    //            }
    //        }
    //    }

    //    if (e.Row.RowType == DataControlRowType.Footer)
    //    {
    //        ImageButton db = (ImageButton)e.Row.FindControl("cmdCompAdd");
    //        if (db != null)
    //        {                
    //                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
    //        }
    //    }
    //    DataRowView drv = (DataRowView)e.Row.DataItem;
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        ImageButton edit = (ImageButton)e.Row.FindControl("cmdCompEdit");
    //        if (edit != null)
    //        {
    //            edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
    //        }

    //        ImageButton save = (ImageButton)e.Row.FindControl("cmdCompSave");
    //        if (save != null)
    //        {
    //            save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);
    //        }

    //        ImageButton cancel = (ImageButton)e.Row.FindControl("cmdCompCancel");
    //        if (cancel != null)
    //        {
    //            cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);
    //        }


    //        ImageButton db = (ImageButton)e.Row.FindControl("cmdCompDelete");
    //        if (db != null)
    //        {
    //            db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
    //            db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
    //        }
    //        RadLabel lbtn = (RadLabel)e.Row.FindControl("lblDescription");
    //        if (lbtn != null)
    //        {
    //            lbtn.Text = (drv["FLDSPECIFICATION"].ToString().Length > 100 ? General.SanitizeHtml(drv["FLDSPECIFICATION"].ToString()).Substring(0, 100) : General.SanitizeHtml(drv["FLDSPECIFICATION"].ToString()));
    //            UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucTooltipDesc");
    //            uct.Text = (drv["FLDSPECIFICATION"].ToString());
    //            lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
    //            lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
    //        }
    //    }
    //}

    protected void gvComponent_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            string componentid = ((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblComponentid")).Text;
            PhoenixDryDockJob.DeleteDryDockJobComponent(General.GetNullableGuid(ViewState["STANDARDJOBID"].ToString()).Value,
                General.GetNullableGuid(componentid).Value, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            _gridView.EditIndex = -1;
            BindDataComponent();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvComponent_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindDataComponent();
    }	

	private bool IsValidRepairJobDetail(string standardjobid,string jobdetail)
	{

		ucError.HeaderMessage = "Please provide the following required information";

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
            ucError.ErrorMessage = "Cannot make changes while you are in ship. Switch to Office to make changes.";

		if (General.GetNullableGuid(standardjobid)==null)
			ucError.ErrorMessage = "Please add Standard job and then add the details";

		if (jobdetail.Trim().Equals(""))
			ucError.ErrorMessage = "Job Detail is required.";

		return (!ucError.IsError);
	}
    private bool IsValidAdditionalJob(string standardjobid, string jobdetail, string orderid)
    {

        ucError.HeaderMessage = "Please provide the following required information";
        
        if (General.GetNullableGuid(standardjobid) == null)
            ucError.ErrorMessage = "Please add Standard job and then add the details";

        if (jobdetail.Trim().Equals(""))
            ucError.ErrorMessage = "Job Detail is required.";

        if (!General.GetNullableGuid(orderid).HasValue)
            ucError.ErrorMessage = "Project is required.";

        return (!ucError.IsError);
    }
    //protected void gvAddDetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;

    //        string additionaljobid = _gridView.DataKeys[nCurrentRow].Value.ToString();
    //        string stdjobid = ViewState["STANDARDJOBID"].ToString();
    //        string details = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDetailEdit")).Text;
    //        string unit = ((UserControlUnit)_gridView.Rows[nCurrentRow].FindControl("ucUnitEdit")).SelectedUnit;
    //        string orderid = ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlProjectEdit")).SelectedValue;
    //        if (!IsValidAdditionalJob(stdjobid, details, orderid))
    //        {
    //            ucError.Visible = true;
    //            return;
    //        }

    //        PhoenixDryDockJobGeneral.UpdateDryDockJobGeneralAdditionalJob(PhoenixSecurityContext.CurrentSecurityContext.VesselID, new Guid(orderid)
    //                    , new Guid(additionaljobid), details, General.GetNullableInteger(unit));

    //        _gridView.EditIndex = -1;
    //        BindAdditionalJobData();

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void gvAddDetail_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;           

            BindAdditionalJobData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void gvAddDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        if (e.CommandName.ToUpper().Equals("SORT"))
    //            return;

    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

    //        if (e.CommandName.ToUpper().Equals("ADD"))
    //        {                
    //            string stdjobid = ViewState["STANDARDJOBID"].ToString();
    //            string details = ((TextBox)_gridView.FooterRow.FindControl("txtDetailAdd")).Text;
    //            string unit = ((UserControlUnit)_gridView.FooterRow.FindControl("ucUnitAdd")).SelectedUnit;
    //            string orderid = ((DropDownList)_gridView.FooterRow.FindControl("ddlProjectAdd")).SelectedValue;
    //            if (!IsValidAdditionalJob(stdjobid, details, orderid))
    //            {
    //                ucError.Visible = true;
    //                return;
    //            }

    //            PhoenixDryDockJobGeneral.InsertDryDockJobGeneralAdditionalJob(PhoenixSecurityContext.CurrentSecurityContext.VesselID, new Guid(stdjobid)
    //                                                , new Guid(orderid), details, General.GetNullableInteger(unit), 1);
    //            BindAdditionalJobData();

    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void gvAddDetail_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
        
        
    //        ImageButton ad = (ImageButton)e.Row.FindControl("cmdAdd");
    //        if (ad != null)
    //        {
    //            ad.Visible = SessionUtil.CanAccess(this.ViewState, ad.CommandName);
    //        }
       

    //        ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
    //        if (edit != null)
    //        {
    //            edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
    //        }

    //        ImageButton save = (ImageButton)e.Row.FindControl("cmdSave");
    //        if (save != null)
    //        {
    //            save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);
    //        }

    //        ImageButton cancel = (ImageButton)e.Row.FindControl("cmdCancel");
    //        if (cancel != null)
    //        {
    //            cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);
    //        }            

    //        ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
    //        if (db != null)
    //        {
    //            db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
    //            db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
    //        }
    //        UserControlUnit ucUnit = (UserControlUnit)e.Row.FindControl("ucUnitEdit");
    //        DataRowView drv = (DataRowView)e.Row.DataItem;
    //        if (ucUnit != null) ucUnit.SelectedUnit = drv["FLDUNIT"].ToString();

    //        DropDownList ddl = (DropDownList)e.Row.FindControl("ddlProjectAdd");
    //        if (ddl != null)
    //        {
    //            BindProject(ddl);
    //        }
    //        ddl = (DropDownList)e.Row.FindControl("ddlProjectEdit");
    //        if (ddl != null)
    //        {
    //            BindProject(ddl);
    //            ddl.SelectedValue = drv["FLDORDERID"].ToString();
    //        }
    //}

    //protected void gvAddDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    try
    //    {
    //        string stdjobid = ViewState["STANDARDJOBID"].ToString();
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;
    //        string additionaljobid = _gridView.DataKeys[nCurrentRow].Value.ToString();
    //        PhoenixDryDockJobGeneral.DeleteDryDockJobGeneralAdditionalJob(PhoenixSecurityContext.CurrentSecurityContext.VesselID,new Guid(stdjobid), new Guid(additionaljobid));

    //        _gridView.EditIndex = -1;
    //        BindAdditionalJobData();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void gvAddDetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindAdditionalJobData();
    }
    private bool IsValidComponent(string componentid)
    {
        
        ucError.HeaderMessage = "Please provide the following required information";

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
            ucError.ErrorMessage = "Cannot add component while you are in office. Switch to ship to add.";

        if (!General.GetNullableGuid(componentid).HasValue)
            ucError.ErrorMessage = "Component is required.";


        return (!ucError.IsError);
    }
    private void BindProject(RadDropDownList ddl)
    {
        ddl.Items.Clear();
        DataTable dt = PhoenixDryDockOrder.ListDryDockOrder(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        ddl.DataTextField = "FLDNUMBER";
        ddl.DataValueField = "FLDORDERID";
        ddl.DataSource = dt;
        ddl.DataBind();
        //ddl.Items.Insert(0, new ListItem("--Select--"));
    }
	private void ShowNoRecordsFound(DataTable dt, GridView gv)
	{
		dt.Rows.Add(dt.NewRow());
		gv.DataSource = dt;
		gv.DataBind();

		int colcount = gv.Columns.Count;
		gv.Rows[0].Cells.Clear();
		gv.Rows[0].Cells.Add(new TableCell());
		gv.Rows[0].Cells[0].ColumnSpan = colcount;
		gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
		gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
		gv.Rows[0].Cells[0].Font.Bold = true;
		gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
		gv.Rows[0].Attributes["onclick"] = "";
	}

    protected void gvComponent_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDataComponent();
    }



    protected void gvComponent_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdCompAdd");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            LinkButton imgComponentAdd = (LinkButton)e.Item.FindControl("imgComponentAdd");
  
            imgComponentAdd.Attributes.Add("onclick", "return showPickList('spnPickListComponentAdd', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListComponent.aspx',true);");
           //  "javascript:openNewWindow('spnPickListVendor', 'codehelp1', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?addresstype=130,131'
        }
        //DataRowView drv = (DataRowView)e.Row.DataItem;
        if(e.Item is GridEditableItem)
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            LinkButton imgComponent = (LinkButton)eeditedItem.FindControl("imgComponent");
            if (imgComponent != null)
                    imgComponent.Attributes.Add("onclick", "return showPickList('spnPickListComponent', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListComponent.aspx',true);");

        }
        if (e.Item is GridDataItem)
        {
            
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdCompEdit");
            if (edit != null)
            {
                edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
            }

            LinkButton save = (LinkButton)e.Item.FindControl("cmdCompSave");
            if (save != null)
            {
                save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);
            }

            LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCompCancel");
            if (cancel != null)
            {
                cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);
            }


            LinkButton db = (LinkButton)e.Item.FindControl("cmdCompDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblDescription");
            if (lbtn != null)
            {
                lbtn.Text = (DataBinder.Eval(e.Item.DataItem, "FLDSPECIFICATION").ToString().Length > 100 ? General.SanitizeHtml(DataBinder.Eval(e.Item.DataItem, "FLDSPECIFICATION").ToString()).Substring(0, 100) : General.SanitizeHtml(DataBinder.Eval(e.Item.DataItem, "FLDSPECIFICATION").ToString()));
                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucTooltipDesc");
                uct.Text = (DataBinder.Eval(e.Item.DataItem, "FLDSPECIFICATION").ToString());
                lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            }
        }
    }

    protected void gvComponent_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            //GridView _gridView = (GridView)sender;
            //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string componentid = ((RadTextBox)e.Item.FindControl("txtComponentIdAdd")).Text;
                if (!IsValidComponent(componentid))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixDryDockJob.UpdateDryDockJobComponent(
                General.GetNullableGuid(ViewState["STANDARDJOBID"].ToString()),
                General.GetNullableGuid(componentid),
                2,
                PhoenixSecurityContext.CurrentSecurityContext.VesselID, null);

                //BindDataComponent();
                gvComponent.DataSource = null;
                gvComponent.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

 

    protected void gvComponent_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            // GridView _gridView = (GridView)sender;
            // int nCurrentRow = e.RowIndex;
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            string componentid = ((RadTextBox)e.Item.FindControl("txtComponentId")).Text;
            string description = ((RadTextBox)e.Item.FindControl("txtDescription")).Text;
            Guid id = (Guid)eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDDTKEY"];
            if (!IsValidComponent(componentid))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixDryDockJob.UpdateDryDockJobComponent(
            General.GetNullableGuid(ViewState["STANDARDJOBID"].ToString()),
            General.GetNullableGuid(componentid),
            2,
            PhoenixSecurityContext.CurrentSecurityContext.VesselID, id, description);

            // _gridView.EditIndex = -1;
            //BindDataComponent();
            gvComponent.DataSource = null;
            gvComponent.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvStandardJobLineItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvStandardJobLineItem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            //GridView _gridView = (GridView)sender;
            //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidRepairJobDetail(ViewState["STANDARDJOBID"].ToString(),
                    ((RadTextBox)e.Item.FindControl("txtDetailAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                Guid? jobdetailid = null;
                jobdetailid = PhoenixDryDockJobGeneral.InsertDryDockJobGeneralDetail(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                 new Guid(ViewState["STANDARDJOBID"].ToString()),
                 ((RadTextBox)e.Item.FindControl("txtDetailAdd")).Text,
                General.GetNullableInteger(((UserControlUnit)e.Item.FindControl("ucUnitAdd")).SelectedUnit),
                ref jobdetailid);
                BindData();
                gvStandardJobLineItem.Rebind();

            }

            if (e.CommandName.ToUpper().Equals("MOVEUP"))
            {

                PhoenixDryDockJobGeneral.MoveDryDockJobGeneralDetailUpOrDown(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(ViewState["STANDARDJOBID"].ToString())
                    , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblDTKey")).Text), 1 // UP
                    );

               // BindData();
                gvStandardJobLineItem.Rebind();
            }

            if (e.CommandName.ToUpper().Equals("MOVEDOWN"))
            {
                PhoenixDryDockJobGeneral.MoveDryDockJobGeneralDetailUpOrDown(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(ViewState["STANDARDJOBID"].ToString())
                    , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblDTKey")).Text), -1 // DOWN
                    );

               // BindData();
                gvStandardJobLineItem.Rebind();
            }

            if(e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixDryDockJobGeneral.DeleteDryDockJobGeneralDetail(new Guid(((RadLabel)e.Item.FindControl("lbljobdetailid")).Text),
                new Guid(ViewState["STANDARDJOBID"].ToString()));

               
                BindData();
                gvStandardJobLineItem.Rebind();
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvStandardJobLineItem_ItemDataBound1(object sender, GridItemEventArgs e)
    {
      

        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }

        if (e.Item is GridDataItem)
        {

            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null)
            {
                edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
            }

            LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
            if (save != null)
            {
                save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);
            }

            LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cancel != null)
            {
                cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);
            }

            LinkButton mup = (LinkButton)e.Item.FindControl("cmdMoveUp");
            if (mup != null)
            {
                mup.Visible = SessionUtil.CanAccess(this.ViewState, mup.CommandName);
            }

            LinkButton mdwn = (LinkButton)e.Item.FindControl("cmdMoveDown");
            if (mdwn != null)
            {
                mdwn.Visible = SessionUtil.CanAccess(this.ViewState, mdwn.CommandName);
            }

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            UserControlUnit ucUnit = (UserControlUnit)e.Item.FindControl("ucUnitEdit");
            // DataRowView drv = (DataRowView)e.Item.DataItem;
            if (ucUnit != null) ucUnit.SelectedUnit = DataBinder.Eval(e.Item.DataItem, "FLDUNIT").ToString();// drv[""].ToString();
            RadCheckBox cb = (RadCheckBox)e.Item.FindControl("chkSelectedYN");
            cb.Enabled = SessionUtil.CanAccess(this.ViewState, "SELECTDETAIL");
            RadButton b = (RadButton)e.Item.FindControl("cmdSelectedYN");
            string jvscript = "";
            if (b != null) jvscript = "javascript:selectJobDetail('" + DataBinder.Eval(e.Item.DataItem, "FLDJOBDETAILID").ToString() + "',this);";
            if (cb != null && b != null) { cb.Attributes.Add("onclick", jvscript); cb.Checked = DataBinder.Eval(e.Item.DataItem, "FLDSELECTEDYN").ToString().Equals("0") ? false : true; }
            if (b != null) b.Attributes.Add("style", "visibility:hidden");
        }
    }

    protected void gvStandardJobLineItem_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            //GridView _gridView = (GridView)sender;
            //int nCurrentRow = e.RowIndex;
            if (!IsValidRepairJobDetail(ViewState["STANDARDJOBID"].ToString(),
                        ((RadTextBox)e.Item.FindControl("txtDetailEdit")).Text))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixDryDockJobGeneral.UpdateDryDockJobGeneralDetail(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                 new Guid(((RadLabel)e.Item.FindControl("lbljobdetailidEdit")).Text),
                 new Guid(ViewState["STANDARDJOBID"].ToString()),
                 ((RadTextBox)e.Item.FindControl("txtDetailEdit")).Text,
                General.GetNullableInteger(((UserControlUnit)e.Item.FindControl("ucUnitEdit")).SelectedUnit));



            // _gridView.EditIndex = -1;
            BindData();
            gvStandardJobLineItem.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAddDetail_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindAdditionalJobData();
    }

    protected void gvAddDetail_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if(e.Item is GridFooterItem)
        {
            LinkButton ad = (LinkButton)e.Item.FindControl("cmdAdd");
            if (ad != null)
            {
                ad.Visible = SessionUtil.CanAccess(this.ViewState, ad.CommandName);
            }
            RadDropDownList ddl = (RadDropDownList)e.Item.FindControl("ddlProjectAdd");
            if (ddl != null)
            {
                ddl.Items.Clear();
                DataTable dt = PhoenixDryDockOrder.ListDryDockOrder(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                ddl.DataTextField = "FLDNUMBER";
                ddl.DataValueField = "FLDORDERID";
                ddl.Items.Add(new DropDownListItem("--Select--", "Dummy"));
                ddl.DataSource = dt;
                ddl.DataBind();
            }
           
        }

        if (e.Item is GridDataItem)
        {


            LinkButton  edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null)
            {
                edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
            }

            LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
            if (save != null)
            {
                save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);
            }

            LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cancel != null)
            {
                cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);
            }

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            UserControlUnit ucUnit = (UserControlUnit)e.Item.FindControl("ucUnitEdit");
        
            if (ucUnit != null) ucUnit.SelectedUnit = DataBinder.Eval(e.Item.DataItem, "FLDUNIT").ToString();


           RadDropDownList  ddl = (RadDropDownList)e.Item.FindControl("ddlProjectEdit");
            if (ddl != null)
            {
                
                DataTable dt = PhoenixDryDockOrder.ListDryDockOrder(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                ddl.DataTextField = "FLDNUMBER";
                ddl.DataValueField = "FLDORDERID";
                ddl.DataSource = dt;
                ddl.DataBind();
                ddl.SelectedValue = DataBinder.Eval(e.Item.DataItem, "FLDORDERID").ToString();
            }
        }

    }

    protected void gvAddDetail_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            //GridView _gridView = (GridView)sender;
            //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string stdjobid = ViewState["STANDARDJOBID"].ToString();
                string details = ((RadTextBox)e.Item.FindControl("txtDetailAdd")).Text;
                string unit = ((UserControlUnit)e.Item.FindControl("ucUnitAdd")).SelectedUnit;
                string orderid = ((RadDropDownList)e.Item.FindControl("ddlProjectAdd")).SelectedValue;
                if (!IsValidAdditionalJob(stdjobid, details, orderid))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixDryDockJobGeneral.InsertDryDockJobGeneralAdditionalJob(PhoenixSecurityContext.CurrentSecurityContext.VesselID, new Guid(stdjobid)
                                                    , new Guid(orderid), details, General.GetNullableInteger(unit), 1);
                BindAdditionalJobData();
                gvAddDetail.Rebind();

            }
            if(e.CommandName.ToUpper().Equals("DELETE"))
            {
                string stdjobid = ViewState["STANDARDJOBID"].ToString();

                GridEditableItem eeditedItem = e.Item as GridEditableItem;

                string additionaljobid = eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDADDITIONALJOBID"].ToString(); ;
                PhoenixDryDockJobGeneral.DeleteDryDockJobGeneralAdditionalJob(PhoenixSecurityContext.CurrentSecurityContext.VesselID, new Guid(stdjobid), new Guid(additionaljobid));

               BindAdditionalJobData();
                gvAddDetail.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAddDetail_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            //GridView _gridView = (GridView)sender;
            //int nCurrentRow = e.RowIndex;
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            
            string additionaljobid = eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDADDITIONALJOBID"].ToString(); ;
            string stdjobid = ViewState["STANDARDJOBID"].ToString();
            string details = ((RadTextBox)e.Item.FindControl("txtDetailEdit")).Text;
            string unit = ((UserControlUnit)e.Item.FindControl("ucUnitEdit")).SelectedUnit;
            string orderid = ((RadDropDownList)e.Item.FindControl("ddlProjectEdit")).SelectedValue;
            if (!IsValidAdditionalJob(stdjobid, details, orderid))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixDryDockJobGeneral.UpdateDryDockJobGeneralAdditionalJob(PhoenixSecurityContext.CurrentSecurityContext.VesselID, new Guid(orderid)
                        , new Guid(additionaljobid), details, General.GetNullableInteger(unit));

           
            BindAdditionalJobData();
            gvAddDetail.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
