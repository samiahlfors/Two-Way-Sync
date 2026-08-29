create table if not exists entity_mapping(
    entity_type varchar(20) not null,
    local_id uuid not null, 
    remote_id int not null,
    last_synced timestamp not null
);

create table if not exists sync(
  entity_type varchar(20) not null,
  last_synced timestamp not null  
);