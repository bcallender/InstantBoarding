// Decompiled with JetBrains decompiler
// Type: Game.Simulation.TransportTrainAISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B32C7D7E-7CA9-498A-8CE0-EA5E934FB7A7
// Assembly location: F:\SteamLibrary\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.City;
using Game.Common;
using Game.Creatures;
using Game.Economy;
using Game.Net;
using Game.Objects;
using Game.Pathfind;
using Game.Prefabs;
using Game.Routes;
using Game.Tools;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using AllAboard.System.Utility;
using Game;
using Game.Simulation;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace AllAboard.System.Patched
{
  [CompilerGenerated]
  public class PatchedTransportTrainAISystem : GameSystemBase
  {
    private EndFrameBarrier m_EndFrameBarrier;
    private SimulationSystem m_SimulationSystem;
    private PathfindSetupSystem m_PathfindSetupSystem;
    private CityStatisticsSystem m_CityStatisticsSystem;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private EntityQuery m_VehicleQuery;
    private EntityQuery m_CarriagePrefabQuery;
    private EntityArchetype m_TransportVehicleRequestArchetype;
    private EntityArchetype m_HandleRequestArchetype;
    private TransportTrainCarriageSelectData m_TransportTrainCarriageSelectData;
    private TransportBoardingHelpers.BoardingLookupData m_BoardingLookupData;
    private Game.Objects.SearchSystem m_ObjectSearchSystem;

    private TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 3;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      
      this.m_PathfindSetupSystem = this.World.GetOrCreateSystemManaged<PathfindSetupSystem>();
      
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      
      this.m_TransportTrainCarriageSelectData = new TransportTrainCarriageSelectData((SystemBase) this);
      
      this.m_BoardingLookupData = new TransportBoardingHelpers.BoardingLookupData((SystemBase) this);
      
      this.m_VehicleQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[5]
        {
          ComponentType.ReadWrite<TrainCurrentLane>(),
          ComponentType.ReadOnly<Owner>(),
          ComponentType.ReadOnly<PrefabRef>(),
          ComponentType.ReadWrite<PathOwner>(),
          ComponentType.ReadWrite<Target>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadWrite<Game.Vehicles.CargoTransport>(),
          ComponentType.ReadWrite<Game.Vehicles.PublicTransport>()
        },
        None = new ComponentType[4]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<TripSource>(),
          ComponentType.ReadOnly<OutOfControl>()
        }
      });
      
      this.m_TransportVehicleRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<ServiceRequest>(), ComponentType.ReadWrite<TransportVehicleRequest>(), ComponentType.ReadWrite<RequestGroup>());
      
      this.m_HandleRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<HandleRequest>(), ComponentType.ReadWrite<Game.Common.Event>());
      
      this.m_CarriagePrefabQuery = this.GetEntityQuery(TransportTrainCarriageSelectData.GetEntityQueryDesc());
      m_ObjectSearchSystem = base.World.GetOrCreateSystemManaged<Game.Objects.SearchSystem>();

      
      this.RequireForUpdate(this.m_VehicleQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      TransportBoardingHelpers.BoardingData boardingData = new TransportBoardingHelpers.BoardingData(Allocator.TempJob);
      JobHandle jobHandle1;
      
      
      
      this.m_TransportTrainCarriageSelectData.PreUpdate((SystemBase) this, this.m_CityConfigurationSystem, this.m_CarriagePrefabQuery, Allocator.TempJob, out jobHandle1);
      
      this.m_BoardingLookupData.Update((SystemBase) this);
      
      
      this.__TypeHandle.__Game_Vehicles_LoadingResources_RW_BufferLookup.Update(ref this.CheckedStateRef);
      
      
      this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferLookup.Update(ref this.CheckedStateRef);
      
      
      this.__TypeHandle.__Game_Vehicles_TrainNavigation_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      
      
      this.__TypeHandle.__Game_Vehicles_TrainCurrentLane_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      
      
      this.__TypeHandle.__Game_Vehicles_Train_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      
      
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      
      
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      
      
      this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup.Update(ref this.CheckedStateRef);
      
      
      this.__TypeHandle.__Game_Economy_Resources_RO_BufferLookup.Update(ref this.CheckedStateRef);
      
      
      this.__TypeHandle.__Game_Vehicles_Passenger_RO_BufferLookup.Update(ref this.CheckedStateRef);
      
      
      this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      
      
      this.__TypeHandle.__Game_Buildings_TransportStation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      
      
      this.__TypeHandle.__Game_Companies_StorageCompany_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      
      
      this.__TypeHandle.__Game_Routes_Color_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      
      
      this.__TypeHandle.__Game_Routes_BoardingVehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      
      
      this.__TypeHandle.__Game_Routes_Connected_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      
      
      this.__TypeHandle.__Game_Routes_Waypoint_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      
      
      this.__TypeHandle.__Game_Prefabs_CargoTransportVehicleData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      
      
      this.__TypeHandle.__Game_Prefabs_PublicTransportVehicleData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      
      
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      
      
      this.__TypeHandle.__Game_Prefabs_TrainData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      
      
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      
      
      this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      
      
      this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      
      
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      
      
      this.__TypeHandle.__Game_Simulation_TransportVehicleRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      
      
      this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      
      
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      
      
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      
      
      this.__TypeHandle.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      
      
      this.__TypeHandle.__Game_Vehicles_TrainNavigationLane_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      
      
      this.__TypeHandle.__Game_Vehicles_LayoutElement_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      
      
      this.__TypeHandle.__Game_Vehicles_Odometer_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      
      
      this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      
      
      this.__TypeHandle.__Game_Common_Target_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      
      
      this.__TypeHandle.__Game_Vehicles_PublicTransport_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      
      
      this.__TypeHandle.__Game_Vehicles_CargoTransport_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      
      
      this.__TypeHandle.__Game_Routes_CurrentRoute_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      
      
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      
      
      this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      
      
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      
      
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      JobHandle jobHandle2 = new TransportTrainTickJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_UnspawnedType = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_CurrentRouteType = this.__TypeHandle.__Game_Routes_CurrentRoute_RO_ComponentTypeHandle,
        m_CargoTransportType = this.__TypeHandle.__Game_Vehicles_CargoTransport_RW_ComponentTypeHandle,
        m_PublicTransportType = this.__TypeHandle.__Game_Vehicles_PublicTransport_RW_ComponentTypeHandle,
        m_TargetType = this.__TypeHandle.__Game_Common_Target_RW_ComponentTypeHandle,
        m_PathOwnerType = this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle,
        m_OdometerType = this.__TypeHandle.__Game_Vehicles_Odometer_RW_ComponentTypeHandle,
        m_LayoutElementType = this.__TypeHandle.__Game_Vehicles_LayoutElement_RW_BufferTypeHandle,
        m_NavigationLaneType = this.__TypeHandle.__Game_Vehicles_TrainNavigationLane_RW_BufferTypeHandle,
        m_ServiceDispatchType = this.__TypeHandle.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_PathInformationData = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup,
        m_TransportVehicleRequestData = this.__TypeHandle.__Game_Simulation_TransportVehicleRequest_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_LaneData = this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup,
        m_EdgeLaneData = this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentLookup,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_PrefabTrainData = this.__TypeHandle.__Game_Prefabs_TrainData_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PublicTransportVehicleData = this.__TypeHandle.__Game_Prefabs_PublicTransportVehicleData_RO_ComponentLookup,
        m_CargoTransportVehicleData = this.__TypeHandle.__Game_Prefabs_CargoTransportVehicleData_RO_ComponentLookup,
        m_WaypointData = this.__TypeHandle.__Game_Routes_Waypoint_RO_ComponentLookup,
        m_ConnectedData = this.__TypeHandle.__Game_Routes_Connected_RO_ComponentLookup,
        m_BoardingVehicleData = this.__TypeHandle.__Game_Routes_BoardingVehicle_RO_ComponentLookup,
        m_RouteColorData = this.__TypeHandle.__Game_Routes_Color_RO_ComponentLookup,
        m_StorageCompanyData = this.__TypeHandle.__Game_Companies_StorageCompany_RO_ComponentLookup,
        m_TransportStationData = this.__TypeHandle.__Game_Buildings_TransportStation_RO_ComponentLookup,
        m_CurrentVehicleData = this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup,
        m_Passengers = this.__TypeHandle.__Game_Vehicles_Passenger_RO_BufferLookup,
        m_EconomyResources = this.__TypeHandle.__Game_Economy_Resources_RO_BufferLookup,
        m_RouteWaypoints = this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup,
        m_ConnectedEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_TrainData = this.__TypeHandle.__Game_Vehicles_Train_RW_ComponentLookup,
        m_CurrentLaneData = this.__TypeHandle.__Game_Vehicles_TrainCurrentLane_RW_ComponentLookup,
        m_NavigationData = this.__TypeHandle.__Game_Vehicles_TrainNavigation_RW_ComponentLookup,
        m_PathElements = this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferLookup,
        m_LoadingResources = this.__TypeHandle.__Game_Vehicles_LoadingResources_RW_BufferLookup,
        m_SimulationFrameIndex = this.m_SimulationSystem.frameIndex,
        m_RandomSeed = RandomSeed.Next(),
        m_TransportVehicleRequestArchetype = this.m_TransportVehicleRequestArchetype,
        m_HandleRequestArchetype = this.m_HandleRequestArchetype,
        m_TransportTrainCarriageSelectData = this.m_TransportTrainCarriageSelectData,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_PathfindQueue = this.m_PathfindSetupSystem.GetQueue((object) this, 64).AsParallelWriter(),
        m_BoardingData = boardingData.ToConcurrent(),
        m_SearchTree = m_ObjectSearchSystem.GetMovingSearchTree(readOnly: false, out var dependencies)
      }.ScheduleParallel<TransportTrainTickJob>(this.m_VehicleQuery, JobHandle.CombineDependencies(this.Dependency, jobHandle1));
      
      
      
      JobHandle inputDeps = boardingData.ScheduleBoarding((SystemBase) this, this.m_CityStatisticsSystem, this.m_BoardingLookupData, this.m_SimulationSystem.frameIndex, jobHandle2);
      
      this.m_TransportTrainCarriageSelectData.PostUpdate(jobHandle2);
      boardingData.Dispose(inputDeps);
      
      
      this.m_PathfindSetupSystem.AddQueueWriter(jobHandle2);
      
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle2);
      this.Dependency = inputDeps;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
    }

    protected override void OnCreateForCompiler()
    {
      base.OnCreateForCompiler();
      
      this.__AssignQueries(ref this.CheckedStateRef);
      
      
      this.__TypeHandle.__AssignHandles(ref this.CheckedStateRef);
    }

    [UnityEngine.Scripting.Preserve]
    public PatchedTransportTrainAISystem()
    {
    }

    [BurstCompile]
    private struct TransportTrainTickJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Unspawned> m_UnspawnedType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<CurrentRoute> m_CurrentRouteType;
      public ComponentTypeHandle<Game.Vehicles.CargoTransport> m_CargoTransportType;
      public ComponentTypeHandle<Game.Vehicles.PublicTransport> m_PublicTransportType;
      public ComponentTypeHandle<Target> m_TargetType;
      public ComponentTypeHandle<PathOwner> m_PathOwnerType;
      public ComponentTypeHandle<Odometer> m_OdometerType;
      public BufferTypeHandle<LayoutElement> m_LayoutElementType;
      public BufferTypeHandle<TrainNavigationLane> m_NavigationLaneType;
      public BufferTypeHandle<ServiceDispatch> m_ServiceDispatchType;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<PathInformation> m_PathInformationData;
      [ReadOnly]
      public ComponentLookup<TransportVehicleRequest> m_TransportVehicleRequestData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Lane> m_LaneData;
      [ReadOnly]
      public ComponentLookup<EdgeLane> m_EdgeLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<TrainData> m_PrefabTrainData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<PublicTransportVehicleData> m_PublicTransportVehicleData;
      [ReadOnly]
      public ComponentLookup<CargoTransportVehicleData> m_CargoTransportVehicleData;
      [ReadOnly]
      public ComponentLookup<Waypoint> m_WaypointData;
      [ReadOnly]
      public ComponentLookup<Connected> m_ConnectedData;
      [ReadOnly]
      public ComponentLookup<BoardingVehicle> m_BoardingVehicleData;
      [ReadOnly]
      public ComponentLookup<Game.Routes.Color> m_RouteColorData;
      [ReadOnly]
      public ComponentLookup<Game.Companies.StorageCompany> m_StorageCompanyData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.TransportStation> m_TransportStationData;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> m_CurrentVehicleData;
      [ReadOnly]
      public BufferLookup<Passenger> m_Passengers;
      [ReadOnly]
      public BufferLookup<Resources> m_EconomyResources;
      [ReadOnly]
      public BufferLookup<RouteWaypoint> m_RouteWaypoints;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_ConnectedEdges;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_SubLanes;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Train> m_TrainData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<TrainCurrentLane> m_CurrentLaneData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<TrainNavigation> m_NavigationData;
      [NativeDisableParallelForRestriction]
      public BufferLookup<PathElement> m_PathElements;
      [NativeDisableParallelForRestriction]
      public BufferLookup<LoadingResources> m_LoadingResources;
      [ReadOnly]
      public uint m_SimulationFrameIndex;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public EntityArchetype m_TransportVehicleRequestArchetype;
      [ReadOnly]
      public EntityArchetype m_HandleRequestArchetype;
      [ReadOnly]
      public TransportTrainCarriageSelectData m_TransportTrainCarriageSelectData;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<SetupQueueItem>.ParallelWriter m_PathfindQueue;
      public TransportBoardingHelpers.BoardingData.Concurrent m_BoardingData;
      public Colossal.Collections.NativeQuadTree<Entity, QuadTreeBoundsXZ> m_SearchTree;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        
        NativeArray<Owner> nativeArray2 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
        
        NativeArray<PrefabRef> nativeArray3 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        
        NativeArray<CurrentRoute> nativeArray4 = chunk.GetNativeArray<CurrentRoute>(ref this.m_CurrentRouteType);
        
        NativeArray<Game.Vehicles.CargoTransport> nativeArray5 = chunk.GetNativeArray<Game.Vehicles.CargoTransport>(ref this.m_CargoTransportType);
        
        NativeArray<Game.Vehicles.PublicTransport> nativeArray6 = chunk.GetNativeArray<Game.Vehicles.PublicTransport>(ref this.m_PublicTransportType);
        
        NativeArray<Target> nativeArray7 = chunk.GetNativeArray<Target>(ref this.m_TargetType);
        
        NativeArray<PathOwner> nativeArray8 = chunk.GetNativeArray<PathOwner>(ref this.m_PathOwnerType);
        
        NativeArray<Odometer> nativeArray9 = chunk.GetNativeArray<Odometer>(ref this.m_OdometerType);
        
        BufferAccessor<LayoutElement> bufferAccessor1 = chunk.GetBufferAccessor<LayoutElement>(ref this.m_LayoutElementType);
        
        BufferAccessor<TrainNavigationLane> bufferAccessor2 = chunk.GetBufferAccessor<TrainNavigationLane>(ref this.m_NavigationLaneType);
        
        BufferAccessor<ServiceDispatch> bufferAccessor3 = chunk.GetBufferAccessor<ServiceDispatch>(ref this.m_ServiceDispatchType);
        
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        
        bool isUnspawned = chunk.Has<Unspawned>(ref this.m_UnspawnedType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity vehicleEntity = nativeArray1[index];
          Owner owner = nativeArray2[index];
          PrefabRef prefabRef = nativeArray3[index];
          PathOwner pathOwner = nativeArray8[index];
          Target target = nativeArray7[index];
          Odometer odometer = nativeArray9[index];
          DynamicBuffer<LayoutElement> layout = bufferAccessor1[index];
          DynamicBuffer<TrainNavigationLane> navigationLanes = bufferAccessor2[index];
          DynamicBuffer<ServiceDispatch> serviceDispatches = bufferAccessor3[index];
          CurrentRoute currentRoute = new CurrentRoute();
          if (nativeArray4.Length != 0)
            currentRoute = nativeArray4[index];
          Game.Vehicles.CargoTransport cargoTransport = new Game.Vehicles.CargoTransport();
          if (nativeArray5.Length != 0)
            cargoTransport = nativeArray5[index];
          Game.Vehicles.PublicTransport publicTransport = new Game.Vehicles.PublicTransport();
          if (nativeArray6.Length != 0)
            publicTransport = nativeArray6[index];
          
          this.Tick(unfilteredChunkIndex, ref random, vehicleEntity, owner, prefabRef, currentRoute, layout, navigationLanes, serviceDispatches, isUnspawned, ref cargoTransport, ref publicTransport, ref pathOwner, ref target, ref odometer);
          nativeArray8[index] = pathOwner;
          nativeArray7[index] = target;
          nativeArray9[index] = odometer;
          if (nativeArray5.Length != 0)
            nativeArray5[index] = cargoTransport;
          if (nativeArray6.Length != 0)
            nativeArray6[index] = publicTransport;
        }
      }

      private void Tick(
        int jobIndex,
        ref Random random,
        Entity vehicleEntity,
        Owner owner,
        PrefabRef prefabRef,
        CurrentRoute currentRoute,
        DynamicBuffer<LayoutElement> layout,
        DynamicBuffer<TrainNavigationLane> navigationLanes,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        bool isUnspawned,
        ref Game.Vehicles.CargoTransport cargoTransport,
        ref Game.Vehicles.PublicTransport publicTransport,
        ref PathOwner pathOwner,
        ref Target target,
        ref Odometer odometer)
      {
        if (VehicleUtils.ResetUpdatedPath(ref pathOwner))
        {
          DynamicBuffer<LoadingResources> bufferData;
          
          if (((cargoTransport.m_State & CargoTransportFlags.DummyTraffic) != (CargoTransportFlags) 0 || (publicTransport.m_State & PublicTransportFlags.DummyTraffic) != (PublicTransportFlags) 0) && this.m_LoadingResources.TryGetBuffer(vehicleEntity, out bufferData))
          {
            if (bufferData.Length != 0)
            {
              
              this.QuantityUpdated(jobIndex, vehicleEntity, layout);
            }
            
            if (this.CheckLoadingResources(jobIndex, ref random, vehicleEntity, true, layout, bufferData))
            {
              pathOwner.m_State |= PathFlags.Updated;
              return;
            }
          }
          cargoTransport.m_State &= ~CargoTransportFlags.Arriving;
          publicTransport.m_State &= ~PublicTransportFlags.Arriving;
          
          DynamicBuffer<PathElement> pathElement = this.m_PathElements[vehicleEntity];
          
          
          float length = VehicleUtils.CalculateLength(vehicleEntity, layout, this.m_PrefabRefData, this.m_PrefabTrainData);
          PathElement prevElement = new PathElement();
          if ((pathOwner.m_State & PathFlags.Append) != (PathFlags) 0)
          {
            if (navigationLanes.Length != 0)
            {
              TrainNavigationLane navigationLane = navigationLanes[navigationLanes.Length - 1];
              prevElement = new PathElement(navigationLane.m_Lane, navigationLane.m_CurvePosition);
            }
          }
          else
          {
            
            
            
            
            if (VehicleUtils.IsReversedPath(pathElement, pathOwner, vehicleEntity, layout, this.m_CurveData, this.m_CurrentLaneData, this.m_TrainData, this.m_TransformData))
            {
              
              
              
              VehicleUtils.ReverseTrain(vehicleEntity, layout, this.m_TrainData, this.m_CurrentLaneData, this.m_NavigationData);
            }
          }
          
          
          
          
          
          
          
          PathUtils.ExtendReverseLocations(prevElement, pathElement, pathOwner, length, this.m_CurveData, this.m_LaneData, this.m_EdgeLaneData, this.m_OwnerData, this.m_EdgeData, this.m_ConnectedEdges, this.m_SubLanes);
          
          
          
          
          if (!this.m_WaypointData.HasComponent(target.m_Target) || this.m_ConnectedData.HasComponent(target.m_Target) && this.m_BoardingVehicleData.HasComponent(this.m_ConnectedData[target.m_Target].m_Connected))
          {
            float distance = length * 0.5f;
            
            
            
            
            
            
            
            PathUtils.ExtendPath(pathElement, pathOwner, ref distance, ref this.m_CurveData, ref this.m_LaneData, ref this.m_EdgeLaneData, ref this.m_OwnerData, ref this.m_EdgeData, ref this.m_ConnectedEdges, ref this.m_SubLanes);
          }
          
          this.UpdatePantograph(layout);
        }
        Entity entity = vehicleEntity;
        if (layout.Length != 0)
          entity = layout[0].m_Vehicle;
        
        TrainCurrentLane currentLane = this.m_CurrentLaneData[entity];
        
        VehicleUtils.CheckUnspawned(jobIndex, vehicleEntity, currentLane, isUnspawned, this.m_CommandBuffer);
        int num = (cargoTransport.m_State & CargoTransportFlags.EnRoute) != (CargoTransportFlags) 0 ? 0 : ((publicTransport.m_State & PublicTransportFlags.EnRoute) == (PublicTransportFlags) 0 ? 1 : 0);
        
        if (this.m_PublicTransportVehicleData.HasComponent(prefabRef.m_Prefab))
        {
          
          PublicTransportVehicleData transportVehicleData = this.m_PublicTransportVehicleData[prefabRef.m_Prefab];
          if ((double) odometer.m_Distance >= (double) transportVehicleData.m_MaintenanceRange && (double) transportVehicleData.m_MaintenanceRange > 0.10000000149011612 && (publicTransport.m_State & PublicTransportFlags.Refueling) == (PublicTransportFlags) 0)
            publicTransport.m_State |= PublicTransportFlags.RequiresMaintenance;
        }
        bool isCargoVehicle = false;
        
        if (this.m_CargoTransportVehicleData.HasComponent(prefabRef.m_Prefab))
        {
          
          CargoTransportVehicleData transportVehicleData = this.m_CargoTransportVehicleData[prefabRef.m_Prefab];
          if ((double) odometer.m_Distance >= (double) transportVehicleData.m_MaintenanceRange && (double) transportVehicleData.m_MaintenanceRange > 0.10000000149011612 && (cargoTransport.m_State & CargoTransportFlags.Refueling) == (CargoTransportFlags) 0)
            cargoTransport.m_State |= CargoTransportFlags.RequiresMaintenance;
          isCargoVehicle = true;
        }
        if (num != 0)
        {
          
          this.CheckServiceDispatches(vehicleEntity, serviceDispatches, ref cargoTransport, ref publicTransport);
          if (serviceDispatches.Length == 0 && (cargoTransport.m_State & (CargoTransportFlags.RequiresMaintenance | CargoTransportFlags.DummyTraffic | CargoTransportFlags.Disabled)) == (CargoTransportFlags) 0 && (publicTransport.m_State & (PublicTransportFlags.RequiresMaintenance | PublicTransportFlags.DummyTraffic | PublicTransportFlags.Disabled)) == (PublicTransportFlags) 0)
          {
            
            this.RequestTargetIfNeeded(jobIndex, vehicleEntity, ref publicTransport, ref cargoTransport);
          }
        }
        else
        {
          serviceDispatches.Clear();
          cargoTransport.m_RequestCount = 0;
          publicTransport.m_RequestCount = 0;
        }
        bool flag = false;
        
        if (!this.m_PrefabRefData.HasComponent(target.m_Target) || VehicleUtils.PathfindFailed(pathOwner))
        {
          if ((cargoTransport.m_State & CargoTransportFlags.Boarding) != (CargoTransportFlags) 0 || (publicTransport.m_State & PublicTransportFlags.Boarding) != (PublicTransportFlags) 0)
          {
            flag = true;
            
            this.StopBoarding(jobIndex, ref random, vehicleEntity, currentRoute, layout, ref cargoTransport, ref publicTransport, ref target, ref odometer, isCargoVehicle, true);
          }
          if (VehicleUtils.IsStuck(pathOwner) || (cargoTransport.m_State & (CargoTransportFlags.Returning | CargoTransportFlags.DummyTraffic)) != (CargoTransportFlags) 0 || (publicTransport.m_State & (PublicTransportFlags.Returning | PublicTransportFlags.DummyTraffic)) != (PublicTransportFlags) 0)
          {
            
            VehicleUtils.DeleteVehicle(this.m_CommandBuffer, jobIndex, vehicleEntity, layout);
            return;
          }
          
          this.ReturnToDepot(jobIndex, vehicleEntity, currentRoute, owner, serviceDispatches, ref cargoTransport, ref publicTransport, ref pathOwner, ref target);
        }
        else if (VehicleUtils.PathEndReached(currentLane))
        {
          if ((cargoTransport.m_State & (CargoTransportFlags.Returning | CargoTransportFlags.DummyTraffic)) != (CargoTransportFlags) 0 || (publicTransport.m_State & (PublicTransportFlags.Returning | PublicTransportFlags.DummyTraffic)) != (PublicTransportFlags) 0)
          {
            if ((cargoTransport.m_State & CargoTransportFlags.Boarding) != (CargoTransportFlags) 0 || (publicTransport.m_State & PublicTransportFlags.Boarding) != (PublicTransportFlags) 0)
            {
              
              if (this.StopBoarding(jobIndex, ref random, vehicleEntity, currentRoute, layout, ref cargoTransport, ref publicTransport, ref target, ref odometer, isCargoVehicle, false))
              {
                flag = true;
                
                if (!this.SelectNextDispatch(jobIndex, vehicleEntity, currentRoute, layout, navigationLanes, serviceDispatches, ref cargoTransport, ref publicTransport, ref currentLane, ref pathOwner, ref target))
                {
                  
                  VehicleUtils.DeleteVehicle(this.m_CommandBuffer, jobIndex, vehicleEntity, layout);
                  return;
                }
              }
            }
            else
            {
              
              
              
              if ((this.CountPassengers(vehicleEntity, layout) <= 0 || !this.StartBoarding(jobIndex, vehicleEntity, currentRoute, prefabRef, ref cargoTransport, ref publicTransport, ref target, isCargoVehicle)) && !this.SelectNextDispatch(jobIndex, vehicleEntity, currentRoute, layout, navigationLanes, serviceDispatches, ref cargoTransport, ref publicTransport, ref currentLane, ref pathOwner, ref target))
              {
                
                VehicleUtils.DeleteVehicle(this.m_CommandBuffer, jobIndex, vehicleEntity, layout);
                return;
              }
            }
          }
          else if ((cargoTransport.m_State & CargoTransportFlags.Boarding) != (CargoTransportFlags) 0 || (publicTransport.m_State & PublicTransportFlags.Boarding) != (PublicTransportFlags) 0)
          {
            
            if (this.StopBoarding(jobIndex, ref random, vehicleEntity, currentRoute, layout, ref cargoTransport, ref publicTransport, ref target, ref odometer, isCargoVehicle, false))
            {
              flag = true;
              if ((cargoTransport.m_State & CargoTransportFlags.EnRoute) == (CargoTransportFlags) 0 && (publicTransport.m_State & PublicTransportFlags.EnRoute) == (PublicTransportFlags) 0)
              {
                
                this.ReturnToDepot(jobIndex, vehicleEntity, currentRoute, owner, serviceDispatches, ref cargoTransport, ref publicTransport, ref pathOwner, ref target);
              }
              else
              {
                
                this.SetNextWaypointTarget(currentRoute, ref pathOwner, ref target);
              }
            }
          }
          else
          {
            
            
            if (!this.m_RouteWaypoints.HasBuffer(currentRoute.m_Route) || !this.m_WaypointData.HasComponent(target.m_Target))
            {
              
              this.ReturnToDepot(jobIndex, vehicleEntity, currentRoute, owner, serviceDispatches, ref cargoTransport, ref publicTransport, ref pathOwner, ref target);
            }
            else
            {
              
              if (!this.StartBoarding(jobIndex, vehicleEntity, currentRoute, prefabRef, ref cargoTransport, ref publicTransport, ref target, isCargoVehicle))
              {
                if ((cargoTransport.m_State & CargoTransportFlags.EnRoute) == (CargoTransportFlags) 0 && (publicTransport.m_State & PublicTransportFlags.EnRoute) == (PublicTransportFlags) 0)
                {
                  
                  this.ReturnToDepot(jobIndex, vehicleEntity, currentRoute, owner, serviceDispatches, ref cargoTransport, ref publicTransport, ref pathOwner, ref target);
                }
                else
                {
                  
                  this.SetNextWaypointTarget(currentRoute, ref pathOwner, ref target);
                }
              }
            }
          }
        }
        else if (VehicleUtils.ReturnEndReached(currentLane))
        {
          
          
          
          VehicleUtils.ReverseTrain(vehicleEntity, layout, this.m_TrainData, this.m_CurrentLaneData, this.m_NavigationData);
          entity = vehicleEntity;
          if (layout.Length != 0)
            entity = layout[0].m_Vehicle;
          
          currentLane = this.m_CurrentLaneData[entity];
          
          this.UpdatePantograph(layout);
        }
        else if ((cargoTransport.m_State & CargoTransportFlags.Boarding) != (CargoTransportFlags) 0 || (publicTransport.m_State & PublicTransportFlags.Boarding) != (PublicTransportFlags) 0)
        {
          flag = true;
          
          this.StopBoarding(jobIndex, ref random, vehicleEntity, currentRoute, layout, ref cargoTransport, ref publicTransport, ref target, ref odometer, isCargoVehicle, true);
        }
        
        Train train = this.m_TrainData[entity];
        train.m_Flags &= ~(Game.Vehicles.TrainFlags.BoardingLeft | Game.Vehicles.TrainFlags.BoardingRight);
        publicTransport.m_State &= ~(PublicTransportFlags.StopLeft | PublicTransportFlags.StopRight);
        Entity skipWaypoint = Entity.Null;
        if ((cargoTransport.m_State & CargoTransportFlags.Boarding) != (CargoTransportFlags) 0 || (publicTransport.m_State & PublicTransportFlags.Boarding) != (PublicTransportFlags) 0)
        {
          if (!flag)
          {
            
            Train controllerTrain = this.m_TrainData[vehicleEntity];
            
            this.UpdateStop(entity, controllerTrain, true, ref train, ref publicTransport, ref target);
          }
        }
        else if ((cargoTransport.m_State & CargoTransportFlags.Returning) != (CargoTransportFlags) 0 || (publicTransport.m_State & PublicTransportFlags.Returning) != (PublicTransportFlags) 0)
        {
          
          if (this.CountPassengers(vehicleEntity, layout) == 0)
          {
            
            this.SelectNextDispatch(jobIndex, vehicleEntity, currentRoute, layout, navigationLanes, serviceDispatches, ref cargoTransport, ref publicTransport, ref currentLane, ref pathOwner, ref target);
          }
        }
        else if ((cargoTransport.m_State & CargoTransportFlags.Arriving) != (CargoTransportFlags) 0 || (publicTransport.m_State & PublicTransportFlags.Arriving) != (PublicTransportFlags) 0)
        {
          
          Train controllerTrain = this.m_TrainData[vehicleEntity];
          
          this.UpdateStop(entity, controllerTrain, false, ref train, ref publicTransport, ref target);
        }
        else
        {
          
          this.CheckNavigationLanes(currentRoute, navigationLanes, ref cargoTransport, ref publicTransport, ref currentLane, ref pathOwner, ref target, out skipWaypoint);
        }
        
        this.FindPathIfNeeded(vehicleEntity, prefabRef, skipWaypoint, ref currentLane, ref cargoTransport, ref publicTransport, ref pathOwner, ref target);
        
        this.m_TrainData[entity] = train;
        
        this.m_CurrentLaneData[entity] = currentLane;
      }

      private void UpdatePantograph(DynamicBuffer<LayoutElement> layout)
      {
        bool flag = false;
        for (int index = 0; index < layout.Length; ++index)
        {
          Entity vehicle = layout[index].m_Vehicle;
          
          Train train = this.m_TrainData[vehicle];
          
          
          TrainData trainData = this.m_PrefabTrainData[this.m_PrefabRefData[vehicle].m_Prefab];
          if (flag || (trainData.m_TrainFlags & Game.Prefabs.TrainFlags.Pantograph) == (Game.Prefabs.TrainFlags) 0)
          {
            train.m_Flags &= ~Game.Vehicles.TrainFlags.Pantograph;
          }
          else
          {
            train.m_Flags |= Game.Vehicles.TrainFlags.Pantograph;
            flag = (trainData.m_TrainFlags & Game.Prefabs.TrainFlags.MultiUnit) != 0;
          }
          
          this.m_TrainData[vehicle] = train;
        }
      }

      private void UpdateStop(
        Entity vehicleEntity,
        Train controllerTrain,
        bool isBoarding,
        ref Train train,
        ref Game.Vehicles.PublicTransport publicTransport,
        ref Target target)
      {
        
        Transform transform = this.m_TransformData[vehicleEntity];
        Connected componentData1;
        Transform componentData2;
        
        
        if (!this.m_ConnectedData.TryGetComponent(target.m_Target, out componentData1) || !this.m_TransformData.TryGetComponent(componentData1.m_Connected, out componentData2))
          return;
        bool flag = (double) math.dot(math.mul(transform.m_Rotation, math.right()), componentData2.m_Position - transform.m_Position) < 0.0;
        if (isBoarding)
        {
          if (flag)
            train.m_Flags |= Game.Vehicles.TrainFlags.BoardingLeft;
          else
            train.m_Flags |= Game.Vehicles.TrainFlags.BoardingRight;
        }
        if (flag ^ ((controllerTrain.m_Flags ^ train.m_Flags) & Game.Vehicles.TrainFlags.Reversed) > (Game.Vehicles.TrainFlags) 0)
          publicTransport.m_State |= PublicTransportFlags.StopLeft;
        else
          publicTransport.m_State |= PublicTransportFlags.StopRight;
      }

      private void FindPathIfNeeded(
        Entity vehicleEntity,
        PrefabRef prefabRef,
        Entity skipWaypoint,
        ref TrainCurrentLane currentLane,
        ref Game.Vehicles.CargoTransport cargoTransport,
        ref Game.Vehicles.PublicTransport publicTransport,
        ref PathOwner pathOwner,
        ref Target target)
      {
        if (!VehicleUtils.RequireNewPath(pathOwner))
          return;
        
        TrainData trainData = this.m_PrefabTrainData[prefabRef.m_Prefab];
        PathfindParameters parameters = new PathfindParameters()
        {
          m_MaxSpeed = (float2) trainData.m_MaxSpeed,
          m_WalkSpeed = (float2) 5.555556f,
          m_Weights = new PathfindWeights(1f, 1f, 1f, 1f),
          m_Methods = PathMethod.Track,
          m_IgnoredRules = RuleFlags.ForbidCombustionEngines | RuleFlags.ForbidHeavyTraffic | RuleFlags.ForbidPrivateTraffic | RuleFlags.ForbidSlowTraffic
        };
        SetupQueueTarget setupQueueTarget = new SetupQueueTarget();
        setupQueueTarget.m_Type = SetupTargetType.CurrentLocation;
        setupQueueTarget.m_Methods = PathMethod.Track;
        setupQueueTarget.m_TrackTypes = trainData.m_TrackType;
        SetupQueueTarget origin = setupQueueTarget;
        setupQueueTarget = new SetupQueueTarget();
        setupQueueTarget.m_Type = SetupTargetType.CurrentLocation;
        setupQueueTarget.m_Methods = PathMethod.Track;
        setupQueueTarget.m_TrackTypes = trainData.m_TrackType;
        setupQueueTarget.m_Entity = target.m_Target;
        SetupQueueTarget destination = setupQueueTarget;
        if (skipWaypoint != Entity.Null)
        {
          origin.m_Entity = skipWaypoint;
          pathOwner.m_State |= PathFlags.Append;
        }
        else
          pathOwner.m_State &= ~PathFlags.Append;
        if ((cargoTransport.m_State & (CargoTransportFlags.EnRoute | CargoTransportFlags.RouteSource)) == (CargoTransportFlags.EnRoute | CargoTransportFlags.RouteSource) || (publicTransport.m_State & (PublicTransportFlags.EnRoute | PublicTransportFlags.RouteSource)) == (PublicTransportFlags.EnRoute | PublicTransportFlags.RouteSource))
          parameters.m_PathfindFlags = PathfindFlags.Stable | PathfindFlags.IgnoreFlow;
        else if ((cargoTransport.m_State & CargoTransportFlags.EnRoute) == (CargoTransportFlags) 0 && (publicTransport.m_State & PublicTransportFlags.EnRoute) == (PublicTransportFlags) 0)
        {
          cargoTransport.m_State &= ~CargoTransportFlags.RouteSource;
          publicTransport.m_State &= ~PublicTransportFlags.RouteSource;
        }
        SetupQueueItem setupQueueItem = new SetupQueueItem(vehicleEntity, parameters, origin, destination);
        
        VehicleUtils.SetupPathfind(ref currentLane, ref pathOwner, this.m_PathfindQueue, setupQueueItem);
      }

      private void CheckNavigationLanes(
        CurrentRoute currentRoute,
        DynamicBuffer<TrainNavigationLane> navigationLanes,
        ref Game.Vehicles.CargoTransport cargoTransport,
        ref Game.Vehicles.PublicTransport publicTransport,
        ref TrainCurrentLane currentLane,
        ref PathOwner pathOwner,
        ref Target target,
        out Entity skipWaypoint)
      {
        skipWaypoint = Entity.Null;
        if (navigationLanes.Length == 0 || navigationLanes.Length >= 10)
          return;
        TrainNavigationLane navigationLane = navigationLanes[navigationLanes.Length - 1];
        if ((navigationLane.m_Flags & TrainLaneFlags.EndOfPath) == (TrainLaneFlags) 0)
          return;
        
        
        
        
        
        if (this.m_WaypointData.HasComponent(target.m_Target) && this.m_RouteWaypoints.HasBuffer(currentRoute.m_Route) && (!this.m_ConnectedData.HasComponent(target.m_Target) || !this.m_BoardingVehicleData.HasComponent(this.m_ConnectedData[target.m_Target].m_Connected)))
        {
          if ((pathOwner.m_State & (PathFlags.Pending | PathFlags.Failed | PathFlags.Obsolete)) != (PathFlags) 0)
            return;
          skipWaypoint = target.m_Target;
          
          this.SetNextWaypointTarget(currentRoute, ref pathOwner, ref target);
          navigationLane.m_Flags &= ~TrainLaneFlags.EndOfPath;
          navigationLanes[navigationLanes.Length - 1] = navigationLane;
          cargoTransport.m_State |= CargoTransportFlags.RouteSource;
          publicTransport.m_State |= PublicTransportFlags.RouteSource;
        }
        else
        {
          cargoTransport.m_State |= CargoTransportFlags.Arriving;
          publicTransport.m_State |= PublicTransportFlags.Arriving;
        }
      }

      private void SetNextWaypointTarget(
        CurrentRoute currentRoute,
        ref PathOwner pathOwnerData,
        ref Target targetData)
      {
        
        DynamicBuffer<RouteWaypoint> routeWaypoint = this.m_RouteWaypoints[currentRoute.m_Route];
        
        int a = this.m_WaypointData[targetData.m_Target].m_Index + 1;
        int index = math.select(a, 0, a >= routeWaypoint.Length);
        VehicleUtils.SetTarget(ref pathOwnerData, ref targetData, routeWaypoint[index].m_Waypoint);
      }

      private void CheckServiceDispatches(
        Entity vehicleEntity,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        ref Game.Vehicles.CargoTransport cargoTransport,
        ref Game.Vehicles.PublicTransport publicTransport)
      {
        if (serviceDispatches.Length > 1)
          serviceDispatches.RemoveRange(1, serviceDispatches.Length - 1);
        cargoTransport.m_RequestCount = math.min(1, cargoTransport.m_RequestCount);
        publicTransport.m_RequestCount = math.min(1, publicTransport.m_RequestCount);
        int index1 = cargoTransport.m_RequestCount + publicTransport.m_RequestCount;
        if (serviceDispatches.Length <= index1)
          return;
        float num = -1f;
        Entity request1 = Entity.Null;
        for (int index2 = index1; index2 < serviceDispatches.Length; ++index2)
        {
          Entity request2 = serviceDispatches[index2].m_Request;
          
          if (this.m_TransportVehicleRequestData.HasComponent(request2))
          {
            
            TransportVehicleRequest transportVehicleRequest = this.m_TransportVehicleRequestData[request2];
            
            if (this.m_PrefabRefData.HasComponent(transportVehicleRequest.m_Route) && (double) transportVehicleRequest.m_Priority > (double) num)
            {
              num = transportVehicleRequest.m_Priority;
              request1 = request2;
            }
          }
        }
        if (request1 != Entity.Null)
        {
          serviceDispatches[index1++] = new ServiceDispatch(request1);
          ++publicTransport.m_RequestCount;
          ++cargoTransport.m_RequestCount;
        }
        if (serviceDispatches.Length <= index1)
          return;
        serviceDispatches.RemoveRange(index1, serviceDispatches.Length - index1);
      }

      private void RequestTargetIfNeeded(
        int jobIndex,
        Entity entity,
        ref Game.Vehicles.PublicTransport publicTransport,
        ref Game.Vehicles.CargoTransport cargoTransport)
      {
        
        
        
        if (this.m_TransportVehicleRequestData.HasComponent(publicTransport.m_TargetRequest) || this.m_TransportVehicleRequestData.HasComponent(cargoTransport.m_TargetRequest) || ((int) this.m_SimulationFrameIndex & (int) math.max(256U, 16U) - 1) != 3)
          return;
        
        
        Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_TransportVehicleRequestArchetype);
        
        this.m_CommandBuffer.SetComponent<ServiceRequest>(jobIndex, entity1, new ServiceRequest(true));
        
        this.m_CommandBuffer.SetComponent<TransportVehicleRequest>(jobIndex, entity1, new TransportVehicleRequest(entity, 1f));
        
        this.m_CommandBuffer.SetComponent<RequestGroup>(jobIndex, entity1, new RequestGroup(8U));
      }

      private bool SelectNextDispatch(
        int jobIndex,
        Entity vehicleEntity,
        CurrentRoute currentRoute,
        DynamicBuffer<LayoutElement> layout,
        DynamicBuffer<TrainNavigationLane> navigationLanes,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        ref Game.Vehicles.CargoTransport cargoTransport,
        ref Game.Vehicles.PublicTransport publicTransport,
        ref TrainCurrentLane currentLane,
        ref PathOwner pathOwner,
        ref Target target)
      {
        if ((cargoTransport.m_State & CargoTransportFlags.Returning) == (CargoTransportFlags) 0 && (publicTransport.m_State & PublicTransportFlags.Returning) == (PublicTransportFlags) 0 && cargoTransport.m_RequestCount + publicTransport.m_RequestCount > 0 && serviceDispatches.Length > 0)
        {
          serviceDispatches.RemoveAt(0);
          cargoTransport.m_RequestCount = math.max(0, cargoTransport.m_RequestCount - 1);
          publicTransport.m_RequestCount = math.max(0, publicTransport.m_RequestCount - 1);
        }
        if ((cargoTransport.m_State & (CargoTransportFlags.RequiresMaintenance | CargoTransportFlags.Disabled)) != (CargoTransportFlags) 0 || (publicTransport.m_State & (PublicTransportFlags.RequiresMaintenance | PublicTransportFlags.Disabled)) != (PublicTransportFlags) 0)
        {
          cargoTransport.m_RequestCount = 0;
          publicTransport.m_RequestCount = 0;
          serviceDispatches.Clear();
          return false;
        }
        for (; cargoTransport.m_RequestCount + publicTransport.m_RequestCount > 0 && serviceDispatches.Length > 0; publicTransport.m_RequestCount = math.max(0, publicTransport.m_RequestCount - 1))
        {
          Entity request = serviceDispatches[0].m_Request;
          Entity route = Entity.Null;
          Entity destination = Entity.Null;
          
          if (this.m_TransportVehicleRequestData.HasComponent(request))
          {
            
            route = this.m_TransportVehicleRequestData[request].m_Route;
            
            if (this.m_PathInformationData.HasComponent(request))
            {
              
              destination = this.m_PathInformationData[request].m_Destination;
            }
          }
          
          if (!this.m_PrefabRefData.HasComponent(destination))
          {
            serviceDispatches.RemoveAt(0);
            cargoTransport.m_RequestCount = math.max(0, cargoTransport.m_RequestCount - 1);
          }
          else
          {
            
            if (this.m_TransportVehicleRequestData.HasComponent(request))
            {
              serviceDispatches.Clear();
              cargoTransport.m_RequestCount = 0;
              publicTransport.m_RequestCount = 0;
              
              if (this.m_PrefabRefData.HasComponent(route))
              {
                if (currentRoute.m_Route != route)
                {
                  
                  this.m_CommandBuffer.AddComponent<CurrentRoute>(jobIndex, vehicleEntity, new CurrentRoute(route));
                  
                  this.m_CommandBuffer.AppendToBuffer<RouteVehicle>(jobIndex, route, new RouteVehicle(vehicleEntity));
                  Game.Routes.Color componentData;
                  
                  if (this.m_RouteColorData.TryGetComponent(route, out componentData))
                  {
                    
                    this.m_CommandBuffer.AddComponent<Game.Routes.Color>(jobIndex, vehicleEntity, componentData);
                    
                    this.UpdateBatches(jobIndex, vehicleEntity, layout);
                  }
                }
                cargoTransport.m_State |= CargoTransportFlags.EnRoute;
                publicTransport.m_State |= PublicTransportFlags.EnRoute;
              }
              else
              {
                
                this.m_CommandBuffer.RemoveComponent<CurrentRoute>(jobIndex, vehicleEntity);
              }
              
              
              Entity entity = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
              
              this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity, new HandleRequest(request, vehicleEntity, true));
            }
            else
            {
              
              this.m_CommandBuffer.RemoveComponent<CurrentRoute>(jobIndex, vehicleEntity);
              
              
              Entity entity = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
              
              this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity, new HandleRequest(request, vehicleEntity, false, true));
            }
            cargoTransport.m_State &= ~CargoTransportFlags.Returning;
            publicTransport.m_State &= ~PublicTransportFlags.Returning;
            
            if (this.m_TransportVehicleRequestData.HasComponent(publicTransport.m_TargetRequest))
            {
              
              
              Entity entity = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
              
              this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity, new HandleRequest(publicTransport.m_TargetRequest, Entity.Null, true));
            }
            
            if (this.m_TransportVehicleRequestData.HasComponent(cargoTransport.m_TargetRequest))
            {
              
              
              Entity entity = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
              
              this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity, new HandleRequest(cargoTransport.m_TargetRequest, Entity.Null, true));
            }
            
            if (this.m_PathElements.HasBuffer(request))
            {
              
              DynamicBuffer<PathElement> pathElement1 = this.m_PathElements[request];
              if (pathElement1.Length != 0)
              {
                
                DynamicBuffer<PathElement> pathElement2 = this.m_PathElements[vehicleEntity];
                PathUtils.TrimPath(pathElement2, ref pathOwner);
                
                float num = math.max(cargoTransport.m_PathElementTime, publicTransport.m_PathElementTime) * (float) pathElement2.Length + this.m_PathInformationData[request].m_Duration;
                if (PathUtils.TryAppendPath(ref currentLane, navigationLanes, pathElement2, pathElement1))
                {
                  cargoTransport.m_PathElementTime = num / (float) math.max(1, pathElement2.Length);
                  publicTransport.m_PathElementTime = cargoTransport.m_PathElementTime;
                  target.m_Target = destination;
                  VehicleUtils.ClearEndOfPath(ref currentLane, navigationLanes);
                  cargoTransport.m_State &= ~CargoTransportFlags.Arriving;
                  publicTransport.m_State &= ~PublicTransportFlags.Arriving;
                  
                  
                  float length = VehicleUtils.CalculateLength(vehicleEntity, layout, this.m_PrefabRefData, this.m_PrefabTrainData);
                  PathElement prevElement = new PathElement();
                  if (navigationLanes.Length != 0)
                  {
                    TrainNavigationLane navigationLane = navigationLanes[navigationLanes.Length - 1];
                    prevElement = new PathElement(navigationLane.m_Lane, navigationLane.m_CurvePosition);
                  }
                  
                  
                  
                  
                  
                  
                  
                  PathUtils.ExtendReverseLocations(prevElement, pathElement2, pathOwner, length, this.m_CurveData, this.m_LaneData, this.m_EdgeLaneData, this.m_OwnerData, this.m_EdgeData, this.m_ConnectedEdges, this.m_SubLanes);
                  
                  
                  
                  
                  if (!this.m_WaypointData.HasComponent(target.m_Target) || this.m_ConnectedData.HasComponent(target.m_Target) && this.m_BoardingVehicleData.HasComponent(this.m_ConnectedData[target.m_Target].m_Connected))
                  {
                    float distance = length * 0.5f;
                    
                    
                    
                    
                    
                    
                    
                    PathUtils.ExtendPath(pathElement2, pathOwner, ref distance, ref this.m_CurveData, ref this.m_LaneData, ref this.m_EdgeLaneData, ref this.m_OwnerData, ref this.m_EdgeData, ref this.m_ConnectedEdges, ref this.m_SubLanes);
                  }
                  return true;
                }
              }
            }
            VehicleUtils.SetTarget(ref pathOwner, ref target, destination);
            return true;
          }
        }
        return false;
      }

      private void UpdateBatches(
        int jobIndex,
        Entity vehicleEntity,
        DynamicBuffer<LayoutElement> layout)
      {
        if (layout.Length != 0)
        {
          
          this.m_CommandBuffer.AddComponent<BatchesUpdated>(jobIndex, layout.Reinterpret<Entity>().AsNativeArray());
        }
        else
        {
          
          this.m_CommandBuffer.AddComponent<BatchesUpdated>(jobIndex, vehicleEntity);
        }
      }

      private void ReturnToDepot(
        int jobIndex,
        Entity vehicleEntity,
        CurrentRoute currentRoute,
        Owner ownerData,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        ref Game.Vehicles.CargoTransport cargoTransport,
        ref Game.Vehicles.PublicTransport publicTransport,
        ref PathOwner pathOwner,
        ref Target target)
      {
        serviceDispatches.Clear();
        cargoTransport.m_RequestCount = 0;
        cargoTransport.m_State &= ~(CargoTransportFlags.EnRoute | CargoTransportFlags.Refueling | CargoTransportFlags.AbandonRoute);
        cargoTransport.m_State |= CargoTransportFlags.Returning;
        publicTransport.m_RequestCount = 0;
        publicTransport.m_State &= ~(PublicTransportFlags.EnRoute | PublicTransportFlags.Refueling | PublicTransportFlags.AbandonRoute);
        publicTransport.m_State |= PublicTransportFlags.Returning;
        
        this.m_CommandBuffer.RemoveComponent<CurrentRoute>(jobIndex, vehicleEntity);
        VehicleUtils.SetTarget(ref pathOwner, ref target, ownerData.m_Owner);
      }

      private bool StartBoarding(
        int jobIndex,
        Entity vehicleEntity,
        CurrentRoute currentRoute,
        PrefabRef prefabRef,
        ref Game.Vehicles.CargoTransport cargoTransport,
        ref Game.Vehicles.PublicTransport publicTransport,
        ref Target target,
        bool isCargoVehicle)
      {
        
        if (this.m_ConnectedData.HasComponent(target.m_Target))
        {
          
          Connected connected = this.m_ConnectedData[target.m_Target];
          
          if (this.m_BoardingVehicleData.HasComponent(connected.m_Connected))
          {
            
            Entity transportStationFromStop = this.GetTransportStationFromStop(connected.m_Connected);
            Entity nextStorageCompany = Entity.Null;
            bool refuel = false;
            
            if (this.m_TransportStationData.HasComponent(transportStationFromStop))
            {
              
              TrainData trainData = this.m_PrefabTrainData[prefabRef.m_Prefab];
              
              refuel = (this.m_TransportStationData[transportStationFromStop].m_TrainRefuelTypes & trainData.m_EnergyType) != 0;
            }
            if (!refuel && ((cargoTransport.m_State & CargoTransportFlags.RequiresMaintenance) != (CargoTransportFlags) 0 || (publicTransport.m_State & PublicTransportFlags.RequiresMaintenance) != (PublicTransportFlags) 0) || (cargoTransport.m_State & CargoTransportFlags.AbandonRoute) != (CargoTransportFlags) 0 || (publicTransport.m_State & PublicTransportFlags.AbandonRoute) != (PublicTransportFlags) 0)
            {
              cargoTransport.m_State &= ~(CargoTransportFlags.EnRoute | CargoTransportFlags.AbandonRoute);
              publicTransport.m_State &= ~(PublicTransportFlags.EnRoute | PublicTransportFlags.AbandonRoute);
              if (currentRoute.m_Route != Entity.Null)
              {
                
                this.m_CommandBuffer.RemoveComponent<CurrentRoute>(jobIndex, vehicleEntity);
              }
            }
            else
            {
              cargoTransport.m_State &= ~CargoTransportFlags.RequiresMaintenance;
              publicTransport.m_State &= ~PublicTransportFlags.RequiresMaintenance;
              cargoTransport.m_State |= CargoTransportFlags.EnRoute;
              publicTransport.m_State |= PublicTransportFlags.EnRoute;
              if (isCargoVehicle)
              {
                
                nextStorageCompany = this.GetNextStorageCompany(currentRoute.m_Route, target.m_Target);
              }
            }
            cargoTransport.m_State |= CargoTransportFlags.RouteSource;
            publicTransport.m_State |= PublicTransportFlags.RouteSource;
            Entity storageCompanyFromStop = Entity.Null;
            if (isCargoVehicle)
            {
              
              storageCompanyFromStop = this.GetStorageCompanyFromStop(connected.m_Connected);
            }
            
            this.m_BoardingData.BeginBoarding(vehicleEntity, currentRoute.m_Route, connected.m_Connected, target.m_Target, storageCompanyFromStop, nextStorageCompany, refuel);
            return true;
          }
        }
        
        if (this.m_WaypointData.HasComponent(target.m_Target))
        {
          cargoTransport.m_State |= CargoTransportFlags.RouteSource;
          publicTransport.m_State |= PublicTransportFlags.RouteSource;
          return false;
        }
        cargoTransport.m_State &= ~(CargoTransportFlags.EnRoute | CargoTransportFlags.AbandonRoute);
        publicTransport.m_State &= ~(PublicTransportFlags.EnRoute | PublicTransportFlags.AbandonRoute);
        if (currentRoute.m_Route != Entity.Null)
        {
          
          this.m_CommandBuffer.RemoveComponent<CurrentRoute>(jobIndex, vehicleEntity);
        }
        return false;
      }

      private bool TryChangeCarriagePrefab(
        int jobIndex,
        ref Random random,
        Entity vehicleEntity,
        bool dummyTraffic,
        DynamicBuffer<LoadingResources> loadingResources)
      {
        
        if (this.m_EconomyResources.HasBuffer(vehicleEntity))
        {
          
          DynamicBuffer<Resources> economyResource = this.m_EconomyResources[vehicleEntity];
          
          PrefabRef prefabRef = this.m_PrefabRefData[vehicleEntity];
          
          if (economyResource.Length == 0 && this.m_CargoTransportVehicleData.HasComponent(prefabRef.m_Prefab))
          {
            while (loadingResources.Length > 0)
            {
              LoadingResources loadingResource = loadingResources[0];
              
              Entity entity = this.m_TransportTrainCarriageSelectData.SelectCarriagePrefab(ref random, loadingResource.m_Resource, loadingResource.m_Amount);
              if (entity != Entity.Null)
              {
                
                CargoTransportVehicleData transportVehicleData = this.m_CargoTransportVehicleData[entity];
                int num = math.min(loadingResource.m_Amount, transportVehicleData.m_CargoCapacity);
                loadingResource.m_Amount -= transportVehicleData.m_CargoCapacity;
                if (loadingResource.m_Amount <= 0)
                  loadingResources.RemoveAt(0);
                else
                  loadingResources[0] = loadingResource;
                if (dummyTraffic)
                {
                  
                  this.m_CommandBuffer.SetBuffer<Resources>(jobIndex, vehicleEntity).Add(new Resources()
                  {
                    m_Resource = loadingResource.m_Resource,
                    m_Amount = num
                  });
                }
                
                this.m_CommandBuffer.SetComponent<PrefabRef>(jobIndex, vehicleEntity, new PrefabRef(entity));
                
                this.m_CommandBuffer.AddComponent<Updated>(jobIndex, vehicleEntity, new Updated());
                return true;
              }
              loadingResources.RemoveAt(0);
            }
          }
        }
        return false;
      }

      private bool CheckLoadingResources(
        int jobIndex,
        ref Random random,
        Entity vehicleEntity,
        bool dummyTraffic,
        DynamicBuffer<LayoutElement> layout,
        DynamicBuffer<LoadingResources> loadingResources)
      {
        bool flag = false;
        if (loadingResources.Length != 0)
        {
          if (layout.Length != 0)
          {
            for (int index = 0; index < layout.Length && loadingResources.Length != 0; ++index)
            {
              
              flag |= this.TryChangeCarriagePrefab(jobIndex, ref random, layout[index].m_Vehicle, dummyTraffic, loadingResources);
            }
          }
          else
          {
            
            flag |= this.TryChangeCarriagePrefab(jobIndex, ref random, vehicleEntity, dummyTraffic, loadingResources);
          }
          loadingResources.Clear();
        }
        return flag;
      }

      private bool StopBoarding(
        int jobIndex,
        ref Random random,
        Entity vehicleEntity,
        CurrentRoute currentRoute,
        DynamicBuffer<LayoutElement> layout,
        ref Game.Vehicles.CargoTransport cargoTransport,
        ref Game.Vehicles.PublicTransport publicTransport,
        ref Target target,
        ref Odometer odometer,
        bool isCargoVehicle,
        bool forcedStop)
      {
        bool flag1 = false;
        
        if (this.m_LoadingResources.HasBuffer(vehicleEntity))
        {
          
          DynamicBuffer<LoadingResources> loadingResource = this.m_LoadingResources[vehicleEntity];
          if (forcedStop)
          {
            loadingResource.Clear();
          }
          else
          {
            bool dummyTraffic = (cargoTransport.m_State & CargoTransportFlags.DummyTraffic) != (CargoTransportFlags) 0 || (publicTransport.m_State & PublicTransportFlags.DummyTraffic) > (PublicTransportFlags) 0;
            
            flag1 |= this.CheckLoadingResources(jobIndex, ref random, vehicleEntity, dummyTraffic, layout, loadingResource);
          }
        }
        if (flag1)
          return false;
        bool flag2 = false;
        Connected componentData1;
        BoardingVehicle componentData2;
        
        
        if (this.m_ConnectedData.TryGetComponent(target.m_Target, out componentData1) && this.m_BoardingVehicleData.TryGetComponent(componentData1.m_Connected, out componentData2))
          flag2 = componentData2.m_Vehicle == vehicleEntity;
        if (!forcedStop)
        {
          
          
          if (flag2 && (this.m_SimulationFrameIndex < cargoTransport.m_DepartureFrame || this.m_SimulationFrameIndex < publicTransport.m_DepartureFrame))
            return false;
          var approxSecondsLate =
            PassengerBoardingChecks.CalculateDwellDelay(m_SimulationFrameIndex, cargoTransport,
              publicTransport);

          if (layout.Length != 0)
            for (var i = 0; i < layout.Length; i++)
            {
              var layoutIndexVehicle = layout[i].m_Vehicle;
              if (m_Passengers.HasBuffer(layoutIndexVehicle))
              {
                var layoutIndexVehiclePassengers = m_Passengers[layoutIndexVehicle];
                if (!PassengerBoardingChecks.ArePassengersReady(layoutIndexVehiclePassengers,
                      m_CurrentVehicleData, m_CommandBuffer, m_SearchTree, approxSecondsLate,
                      jobIndex))
                  return false;
              }
            }
          else if (!m_Passengers.HasBuffer(vehicleEntity))
            return true;
          else if (!PassengerBoardingChecks.ArePassengersReady(m_Passengers[vehicleEntity],
                     m_CurrentVehicleData, m_CommandBuffer, m_SearchTree, approxSecondsLate, jobIndex))
            return false;
        }
        if ((cargoTransport.m_State & CargoTransportFlags.Refueling) != (CargoTransportFlags) 0 || (publicTransport.m_State & PublicTransportFlags.Refueling) != (PublicTransportFlags) 0)
          odometer.m_Distance = 0.0f;
        if (isCargoVehicle)
        {
          
          this.QuantityUpdated(jobIndex, vehicleEntity, layout);
        }
        if (flag2)
        {
          Entity storageCompanyFromStop = Entity.Null;
          Entity nextStorageCompany = Entity.Null;
          if (isCargoVehicle && !forcedStop)
          {
            
            storageCompanyFromStop = this.GetStorageCompanyFromStop(componentData1.m_Connected);
            if ((cargoTransport.m_State & CargoTransportFlags.EnRoute) != (CargoTransportFlags) 0)
            {
              
              nextStorageCompany = this.GetNextStorageCompany(currentRoute.m_Route, target.m_Target);
            }
          }
          
          this.m_BoardingData.EndBoarding(vehicleEntity, currentRoute.m_Route, componentData1.m_Connected, target.m_Target, storageCompanyFromStop, nextStorageCompany);
          return true;
        }
        cargoTransport.m_State &= ~(CargoTransportFlags.Boarding | CargoTransportFlags.Refueling);
        publicTransport.m_State &= ~(PublicTransportFlags.Boarding | PublicTransportFlags.Refueling);
        return true;
      }

      private void QuantityUpdated(
        int jobIndex,
        Entity vehicleEntity,
        DynamicBuffer<LayoutElement> layout)
      {
        if (layout.Length != 0)
        {
          for (int index = 0; index < layout.Length; ++index)
          {
            
            this.m_CommandBuffer.AddComponent<Updated>(jobIndex, layout[index].m_Vehicle, new Updated());
          }
        }
        else
        {
          
          this.m_CommandBuffer.AddComponent<Updated>(jobIndex, vehicleEntity, new Updated());
        }
      }

      private int CountPassengers(Entity vehicleEntity, DynamicBuffer<LayoutElement> layout)
      {
        int num = 0;
        if (layout.Length != 0)
        {
          for (int index = 0; index < layout.Length; ++index)
          {
            Entity vehicle = layout[index].m_Vehicle;
            
            if (this.m_Passengers.HasBuffer(vehicle))
            {
              
              num += this.m_Passengers[vehicle].Length;
            }
          }
        }
        else
        {
          
          if (this.m_Passengers.HasBuffer(vehicleEntity))
          {
            
            num += this.m_Passengers[vehicleEntity].Length;
          }
        }
        return num;
      }
      

      private Entity GetTransportStationFromStop(Entity stop)
      {
        
        
        for (; !this.m_TransportStationData.HasComponent(stop); stop = this.m_OwnerData[stop].m_Owner)
        {
          
          if (!this.m_OwnerData.HasComponent(stop))
            return Entity.Null;
        }
        
        if (this.m_OwnerData.HasComponent(stop))
        {
          
          Entity owner = this.m_OwnerData[stop].m_Owner;
          
          if (this.m_TransportStationData.HasComponent(owner))
            return owner;
        }
        return stop;
      }

      private Entity GetStorageCompanyFromStop(Entity stop)
      {
        
        
        for (; !this.m_StorageCompanyData.HasComponent(stop); stop = this.m_OwnerData[stop].m_Owner)
        {
          
          if (!this.m_OwnerData.HasComponent(stop))
            return Entity.Null;
        }
        return stop;
      }

      private Entity GetNextStorageCompany(Entity route, Entity currentWaypoint)
      {
        
        DynamicBuffer<RouteWaypoint> routeWaypoint = this.m_RouteWaypoints[route];
        
        int a = this.m_WaypointData[currentWaypoint].m_Index + 1;
        for (int index1 = 0; index1 < routeWaypoint.Length; ++index1)
        {
          int index2 = math.select(a, 0, a >= routeWaypoint.Length);
          Entity waypoint = routeWaypoint[index2].m_Waypoint;
          
          if (this.m_ConnectedData.HasComponent(waypoint))
          {
            
            
            Entity storageCompanyFromStop = this.GetStorageCompanyFromStop(this.m_ConnectedData[waypoint].m_Connected);
            if (storageCompanyFromStop != Entity.Null)
              return storageCompanyFromStop;
          }
          a = index2 + 1;
        }
        return Entity.Null;
      }

      void IJobChunk.Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        
        this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Unspawned> __Game_Objects_Unspawned_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CurrentRoute> __Game_Routes_CurrentRoute_RO_ComponentTypeHandle;
      public ComponentTypeHandle<Game.Vehicles.CargoTransport> __Game_Vehicles_CargoTransport_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Game.Vehicles.PublicTransport> __Game_Vehicles_PublicTransport_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Target> __Game_Common_Target_RW_ComponentTypeHandle;
      public ComponentTypeHandle<PathOwner> __Game_Pathfind_PathOwner_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Odometer> __Game_Vehicles_Odometer_RW_ComponentTypeHandle;
      public BufferTypeHandle<LayoutElement> __Game_Vehicles_LayoutElement_RW_BufferTypeHandle;
      public BufferTypeHandle<TrainNavigationLane> __Game_Vehicles_TrainNavigationLane_RW_BufferTypeHandle;
      public BufferTypeHandle<ServiceDispatch> __Game_Simulation_ServiceDispatch_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathInformation> __Game_Pathfind_PathInformation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TransportVehicleRequest> __Game_Simulation_TransportVehicleRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Lane> __Game_Net_Lane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EdgeLane> __Game_Net_EdgeLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrainData> __Game_Prefabs_TrainData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PublicTransportVehicleData> __Game_Prefabs_PublicTransportVehicleData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CargoTransportVehicleData> __Game_Prefabs_CargoTransportVehicleData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Waypoint> __Game_Routes_Waypoint_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Connected> __Game_Routes_Connected_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BoardingVehicle> __Game_Routes_BoardingVehicle_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Routes.Color> __Game_Routes_Color_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Companies.StorageCompany> __Game_Companies_StorageCompany_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.TransportStation> __Game_Buildings_TransportStation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> __Game_Creatures_CurrentVehicle_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Passenger> __Game_Vehicles_Passenger_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Resources> __Game_Economy_Resources_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<RouteWaypoint> __Game_Routes_RouteWaypoint_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;
      public ComponentLookup<Train> __Game_Vehicles_Train_RW_ComponentLookup;
      public ComponentLookup<TrainCurrentLane> __Game_Vehicles_TrainCurrentLane_RW_ComponentLookup;
      public ComponentLookup<TrainNavigation> __Game_Vehicles_TrainNavigation_RW_ComponentLookup;
      public BufferLookup<PathElement> __Game_Pathfind_PathElement_RW_BufferLookup;
      public BufferLookup<LoadingResources> __Game_Vehicles_LoadingResources_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        
        this.__Game_Objects_Unspawned_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Unspawned>(true);
        
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        
        this.__Game_Routes_CurrentRoute_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentRoute>(true);
        
        this.__Game_Vehicles_CargoTransport_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Vehicles.CargoTransport>();
        
        this.__Game_Vehicles_PublicTransport_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Vehicles.PublicTransport>();
        
        this.__Game_Common_Target_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Target>();
        
        this.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PathOwner>();
        
        this.__Game_Vehicles_Odometer_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Odometer>();
        
        this.__Game_Vehicles_LayoutElement_RW_BufferTypeHandle = state.GetBufferTypeHandle<LayoutElement>();
        
        this.__Game_Vehicles_TrainNavigationLane_RW_BufferTypeHandle = state.GetBufferTypeHandle<TrainNavigationLane>();
        
        this.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle = state.GetBufferTypeHandle<ServiceDispatch>();
        
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        
        this.__Game_Pathfind_PathInformation_RO_ComponentLookup = state.GetComponentLookup<PathInformation>(true);
        
        this.__Game_Simulation_TransportVehicleRequest_RO_ComponentLookup = state.GetComponentLookup<TransportVehicleRequest>(true);
        
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        
        this.__Game_Net_Lane_RO_ComponentLookup = state.GetComponentLookup<Lane>(true);
        
        this.__Game_Net_EdgeLane_RO_ComponentLookup = state.GetComponentLookup<EdgeLane>(true);
        
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Edge>(true);
        
        this.__Game_Prefabs_TrainData_RO_ComponentLookup = state.GetComponentLookup<TrainData>(true);
        
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        
        this.__Game_Prefabs_PublicTransportVehicleData_RO_ComponentLookup = state.GetComponentLookup<PublicTransportVehicleData>(true);
        
        this.__Game_Prefabs_CargoTransportVehicleData_RO_ComponentLookup = state.GetComponentLookup<CargoTransportVehicleData>(true);
        
        this.__Game_Routes_Waypoint_RO_ComponentLookup = state.GetComponentLookup<Waypoint>(true);
        
        this.__Game_Routes_Connected_RO_ComponentLookup = state.GetComponentLookup<Connected>(true);
        
        this.__Game_Routes_BoardingVehicle_RO_ComponentLookup = state.GetComponentLookup<BoardingVehicle>(true);
        
        this.__Game_Routes_Color_RO_ComponentLookup = state.GetComponentLookup<Game.Routes.Color>(true);
        
        this.__Game_Companies_StorageCompany_RO_ComponentLookup = state.GetComponentLookup<Game.Companies.StorageCompany>(true);
        
        this.__Game_Buildings_TransportStation_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.TransportStation>(true);
        
        this.__Game_Creatures_CurrentVehicle_RO_ComponentLookup = state.GetComponentLookup<CurrentVehicle>(true);
        
        this.__Game_Vehicles_Passenger_RO_BufferLookup = state.GetBufferLookup<Passenger>(true);
        
        this.__Game_Economy_Resources_RO_BufferLookup = state.GetBufferLookup<Resources>(true);
        
        this.__Game_Routes_RouteWaypoint_RO_BufferLookup = state.GetBufferLookup<RouteWaypoint>(true);
        
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
        
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
        
        this.__Game_Vehicles_Train_RW_ComponentLookup = state.GetComponentLookup<Train>();
        
        this.__Game_Vehicles_TrainCurrentLane_RW_ComponentLookup = state.GetComponentLookup<TrainCurrentLane>();
        
        this.__Game_Vehicles_TrainNavigation_RW_ComponentLookup = state.GetComponentLookup<TrainNavigation>();
        
        this.__Game_Pathfind_PathElement_RW_BufferLookup = state.GetBufferLookup<PathElement>();
        
        this.__Game_Vehicles_LoadingResources_RW_BufferLookup = state.GetBufferLookup<LoadingResources>();
      }
    }
  }
}
