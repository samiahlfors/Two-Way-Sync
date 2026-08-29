drop table if exists entity_mapping;
drop table if exists sync;

create table entity_mapping(
    entity_type varchar(20) not null,
    local_id uuid not null, 
    remote_id int not null,
    last_synced timestamp not null
);

create table sync(
  entity_type varchar(20) not null,
  last_synced timestamp not null  
);