export interface Client {
  id:string
  name: string;
  address: string;
  description: string;
  jobTitle: string;
  clientClassification : string;
  clientSource : string;
  salesMan : string;
  createdById : string;
  creationDate : Date;
  modifiedById? : string;
  modificationDate? : Date;
}
