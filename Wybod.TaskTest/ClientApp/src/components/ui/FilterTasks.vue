<template>
  <div class="flex gap-3 items-center mb-4">
    <!-- Search Input -->
    <input
      v-model="search"
      @input="emitChange"
      type="text"
      placeholder="Search tasks..."
      class="px-3 py-2 w-full border rounded-lg outline-none focus:ring-2 focus:ring-blue-400 focus:border-blue-400 transition-all placeholder-gray-400"
    />

    <!-- Status Filter -->
    <select
      v-model="status"
      @change="emitChange"
      class="px-3 py-2 border rounded-lg outline-none hover:cursor focus:ring-2 focus:ring-blue-400 focus:border-blue-400 transition-all bg-white cursor-pointer"
    >
        <option :value="Status.All">All</option>
        <option :value="Status.Pending">Pending</option>
        <option :value="Status.Completed">Completed</option>
    </select>

    <!-- Reset Button -->
    <button
      @click="reset"
      class="px-4 py-2 rounded-lg bg-gray-100 text-gray-700 hover:bg-gray-400 hover:text-white focus:ring-2 focus:ring-blue-400 transition-all cursor-pointer"
    >
      Reset
    </button>
  </div>
</template>

<script setup lang="ts">
import { Status } from '@/lib/enums/status';
import { ref } from 'vue'

interface Filters {
  search: string
  status: Status
}

const props = defineProps<{ initial?: Partial<Filters> }>()
const emit = defineEmits<{ (e: 'change', value: Filters): void }>()

const search = ref(props.initial?.search ?? '')
const status = ref<Status>((props.initial?.status as Status) ?? 'all')

const emitChange = () => {
  emit('change', { search: search.value.trim(), status: status.value })
}

const reset = () => {
  search.value = ''
  status.value = Status.All
  emitChange()
}
</script>
