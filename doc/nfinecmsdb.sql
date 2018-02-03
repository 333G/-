/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2008                    */
/* Created on:     2017/4/22 14:49:54                           */
/*==============================================================*/


if exists (select 1
            from  sysobjects
           where  id = object_id('dbo.tmp_SMC_SendSms')
            and   type = 'U')
   drop table dbo.tmp_SMC_SendSms
go

execute sp_rename SMC_SendSms, tmp_SMC_SendSms
go

/*==============================================================*/
/* Table: SMC_SendSms                                           */
/*==============================================================*/
create table dbo.SMC_SendSms (
   F_Id                 varchar(50)          collate Chinese_PRC_CI_AS not null,
   F_MobileList         text                 collate Chinese_PRC_CI_AS null,
   F_MobileCount        int                  null,
   F_SmsContent         varchar(500)         collate Chinese_PRC_CI_AS null,
   F_IsTimer            bit                  null constraint DF_SMC_SendSms_Is_Timer default (0),
   F_SendTime           datetime             null,
   F_SendSign           varchar(32)          collate Chinese_PRC_CI_AS null,
   F_SmbSign            varchar(20)          collate Chinese_PRC_CI_AS null,
   F_SendState          int                  null,
   F_DealState          int                  null,
   F_OperateState       int                  null,
   F_Price              decimal(18,2)        null,
   F_GroupChannelId     varchar(50)          null,
   F_RootId             int                  null,
   F_DeleteMark         bit                  null,
   F_EnabledMark        bit                  null,
   F_CreatorTime        datetime             null constraint DF_SMC_SendSms_F_CreatorTime default getdate(),
   F_CreatorUserId      varchar(50)          collate Chinese_PRC_CI_AS null,
   F_LastModifyTime     datetime             null,
   F_LastModifyUserId   varchar(50)          collate Chinese_PRC_CI_AS null,
   F_DeleteTime         datetime             null,
   F_DeleteUserId       varchar(50)          collate Chinese_PRC_CI_AS null,
   constraint PK__ocm_SendSms primary key (F_Id)
         on "PRIMARY"
)
on "PRIMARY"
go

if exists (select 1 from  sys.extended_properties
           where major_id = object_id('dbo.SMC_SendSms') and minor_id = 0)
begin 
   execute sp_dropextendedproperty 'MS_Description',  
   'user', 'dbo', 'table', 'SMC_SendSms' 
 
end 


execute sp_addextendedproperty 'MS_Description',  
   '���Ͷ��ű�', 
   'user', 'dbo', 'table', 'SMC_SendSms'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_Id')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_Id'

end


execute sp_addextendedproperty 'MS_Description', 
   '���',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_Id'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_MobileList')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_MobileList'

end


execute sp_addextendedproperty 'MS_Description', 
   '�ռ��˺����ַ������ָ���������',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_MobileList'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_MobileCount')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_MobileCount'

end


execute sp_addextendedproperty 'MS_Description', 
   '���ź�������',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_MobileCount'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_SmsContent')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_SmsContent'

end


execute sp_addextendedproperty 'MS_Description', 
   '��������',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_SmsContent'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_IsTimer')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_IsTimer'

end


execute sp_addextendedproperty 'MS_Description', 
   '�Ƿ�ʱ��Ĭ����false,false=��ʱ���ͣ�true=��ʱ���ͣ�',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_IsTimer'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_SendTime')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_SendTime'

end


execute sp_addextendedproperty 'MS_Description', 
   '����ʱ�䣬���ڶ�ʱ����',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_SendTime'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_SendSign')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_SendSign'

end


execute sp_addextendedproperty 'MS_Description', 
   '���ű������',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_SendSign'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_SmbSign')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_SmbSign'

end


execute sp_addextendedproperty 'MS_Description', 
   '���ű��Ӣ��',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_SmbSign'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_SendState')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_SendState'

end


execute sp_addextendedproperty 'MS_Description', 
   '����״̬(0=�����ͣ�9=���ͳɹ���-1=����ʧ�ܣ�)',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_SendState'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_DealState')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_DealState'

end


execute sp_addextendedproperty 'MS_Description', 
   '���״̬(0=δ��ˣ�9=����ˣ�-1=����ˣ�-2=δͨ��)',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_DealState'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_OperateState')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_OperateState'

end


execute sp_addextendedproperty 'MS_Description', 
   '����״̬(0=������9=�Ѵ���1=������)',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_OperateState'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_Price')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_Price'

end


execute sp_addextendedproperty 'MS_Description', 
   '����',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_Price'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_GroupChannelId')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_GroupChannelId'

end


execute sp_addextendedproperty 'MS_Description', 
   '����ͨ��',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_GroupChannelId'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_RootId')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_RootId'

end


execute sp_addextendedproperty 'MS_Description', 
   '����ID',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_RootId'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_DeleteMark')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_DeleteMark'

end


execute sp_addextendedproperty 'MS_Description', 
   'ɾ�����',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_DeleteMark'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_EnabledMark')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_EnabledMark'

end


execute sp_addextendedproperty 'MS_Description', 
   '���ñ��',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_EnabledMark'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_CreatorTime')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_CreatorTime'

end


execute sp_addextendedproperty 'MS_Description', 
   '����ʱ��',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_CreatorTime'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_CreatorUserId')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_CreatorUserId'

end


execute sp_addextendedproperty 'MS_Description', 
   '������',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_CreatorUserId'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_LastModifyTime')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_LastModifyTime'

end


execute sp_addextendedproperty 'MS_Description', 
   '���༭ʱ��',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_LastModifyTime'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_LastModifyUserId')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_LastModifyUserId'

end


execute sp_addextendedproperty 'MS_Description', 
   '���༭��',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_LastModifyUserId'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_DeleteTime')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_DeleteTime'

end


execute sp_addextendedproperty 'MS_Description', 
   'ɾ��ʱ��',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_DeleteTime'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_DeleteUserId')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_DeleteUserId'

end


execute sp_addextendedproperty 'MS_Description', 
   'ɾ����',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_DeleteUserId'
go

insert into dbo.SMC_SendSms (F_Id, F_MobileList, F_MobileCount, F_SmsContent, F_SendTime, F_SendSign, F_SmbSign, F_SendState, F_DealState, F_Price, F_GroupChannelId, F_RootId, F_DeleteMark, F_EnabledMark, F_CreatorTime, F_CreatorUserId, F_LastModifyTime, F_LastModifyUserId, F_DeleteTime, F_DeleteUserId)
select F_Id, F_MobileList, F_MobileCount, F_SmsContent, F_SendTime, F_SendSign, F_SmbSign, F_SendState, F_DealState, F_Price, F_GroupChannelId, F_RootId, F_DeleteMark, F_EnabledMark, F_CreatorTime, F_CreatorUserId, F_LastModifyTime, F_LastModifyUserId, F_DeleteTime, F_DeleteUserId
from dbo.tmp_SMC_SendSms
go

alter table Sev_SendDateDetail
   add constraint FK_SEV_SEND_REFERENCE_SMC_SEND foreign key (SMC_F_Id)
      references dbo.SMC_SendSms (F_Id)
go

