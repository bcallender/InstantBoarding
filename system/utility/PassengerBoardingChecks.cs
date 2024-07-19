using Colossal.Collections;
using Colossal.Logging;
using Game.Common;
using Game.Creatures;
using Game.Vehicles;
using Unity.Entities;
using UnityEngine;

namespace InstantBoarding
{
    public static class PassengerBoardingChecks
    {

        public static uint CalculateDwellDelay(uint simulationFrameIndex, Game.Vehicles.CargoTransport cargoTransport, Game.Vehicles.PublicTransport publicTransport)
        {
            var intendedDepartureFrame = simulationFrameIndex > publicTransport.m_DepartureFrame
                ? publicTransport.m_DepartureFrame
                : cargoTransport.m_DepartureFrame;
            var numberOfFramesLate = simulationFrameIndex - intendedDepartureFrame;
            var approxSecondsLate = numberOfFramesLate / 60U;
            return approxSecondsLate;
        }
        
        public static bool ArePassengersReady(DynamicBuffer<Passenger> passengers,
            ComponentLookup<CurrentVehicle> currentVehicleData,
            EntityCommandBuffer.ParallelWriter commandBuffer,
            Colossal.Collections.NativeQuadTree<Entity, QuadTreeBoundsXZ> searchTree,
            uint approxSecondsLate,
            uint maxAllowedSecondsLate, int jobIndex) {

            return approxSecondsLate > maxAllowedSecondsLate
                ? //how tolerant are you for your trains?
                BruteForceBoarding(passengers, currentVehicleData, commandBuffer, searchTree, jobIndex)
                : AreAllPassengersBoarded(passengers, currentVehicleData);
        }

        private static bool BruteForceBoarding(DynamicBuffer<Passenger> passengers,
            ComponentLookup<CurrentVehicle> currentVehicleDataLookup,
            EntityCommandBuffer.ParallelWriter commandBuffer, NativeQuadTree<Entity, QuadTreeBoundsXZ> searchTree,
            int jobIndex)
        {
            
            for (int i = 0; i < passengers.Length; i++)
            {
                Entity passenger = passengers[i].m_Passenger;
                if (currentVehicleDataLookup.HasComponent(passenger))
                {
                    continue;
                }


                if ((currentVehicleDataLookup[passenger].m_Flags & CreatureVehicleFlags.Entering) != 0)
                {
                    if (currentVehicleDataLookup.TryGetComponent(passenger, out var currentVehicleData))
                    {
                        currentVehicleData.m_Flags |= CreatureVehicleFlags.Ready;
                        currentVehicleData.m_Flags &= ~CreatureVehicleFlags.Entering;
                        commandBuffer.SetComponent(jobIndex, passenger, currentVehicleData);
                        commandBuffer.AddComponent(jobIndex, passenger, default(BatchesUpdated));
                        searchTree.TryRemove(passenger);
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static bool AreAllPassengersBoarded(DynamicBuffer<Passenger> passengers,
            ComponentLookup<CurrentVehicle> currentVehicleData)
        {
            for (int index = 0; index < passengers.Length; ++index)
            {
                Entity passenger2 = passengers[index].m_Passenger;
                if (currentVehicleData.HasComponent(passenger2) &&
                    (currentVehicleData[passenger2].m_Flags & CreatureVehicleFlags.Ready) ==
                    (CreatureVehicleFlags)0)
                    return false;
            }

            return true;
        }

    }


}